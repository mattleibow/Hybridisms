function Process-OnnxFile {
    param(
        [Parameter(Mandatory=$true)]
        [object]$OnnxFile,
        [Parameter(Mandatory=$true)]
        [string]$OnnxParent
    )
    $rootFiles = Get-ChildItem -Path $OnnxParent -File
    $folderName = [System.IO.Path]::GetFileNameWithoutExtension($OnnxFile.Name)
    $targetDir = Join-Path (Split-Path $OnnxParent -Parent) $folderName
    if (-not (Test-Path $targetDir)) {
        New-Item -ItemType Directory -Path $targetDir | Out-Null
    }

    Write-Host "[INFO] Copying files in $OnnxParent to $targetDir"
    foreach ($file in $rootFiles) {
        Copy-Item $file.FullName -Destination $targetDir
    }

    Write-Host "[INFO] Copying $($OnnxFile.Name) to $targetDir as model.onnx"
    Copy-Item $OnnxFile.FullName -Destination (Join-Path $targetDir "model.onnx")
    
    Write-Host "[INFO] Generating genai_config.json for $folderName"
    python -m onnxruntime_genai.models.builder `
        -m $targetDir `
        -e cpu `
        -p int4 `
        --extra_options config_only=true `
        -o $targetDir

    $zipPath = Join-Path (Split-Path $targetDir -Parent) ("$folderName.zip")
    Write-Host "[INFO] Zipping $targetDir to $zipPath"
    if (Test-Path $zipPath) { Remove-Item $zipPath }
    Compress-Archive -Path $targetDir\* -DestinationPath $zipPath
}

# Example usage:
# Download-And-Prepare-Model -ModelName "onnx-community/Qwen2.5-0.5B-Instruct"
# Download-And-Prepare-Model -ModelName "Qwen/Qwen2.5-1.5B-Instruct" -Quantization "uint8"
function Download-And-Prepare-Model {
    param(
        [Parameter(Mandatory=$true)]
        [string]$ModelName,
        [Parameter(Mandatory=$false)]
        [string]$Quantization = ""
    )
    Write-Host "[INFO] Starting download and preparation for model: $ModelName"

    # Extract the short model name (after the last slash)
    $ShortName = ($ModelName -split '/')[1]

    # Download the model
    $onnxParent = Join-Path $PSScriptRoot "../artifacts/models/$ShortName/Base"
    Write-Host "[INFO] Downloading model to: $onnxParent"
    huggingface-cli download $ModelName --local-dir $onnxParent

    # Process ONNX files if present
    $onnxSubfolder = Join-Path $onnxParent "onnx"
    if (Test-Path $onnxSubfolder) {
        Write-Host "[INFO] Found ONNX subfolder: $onnxSubfolder"

        if ($Quantization -ne "") {
            $targetOnnx = "model_$Quantization.onnx"
            $onnxFile = Get-ChildItem -Path $onnxSubfolder -Filter $targetOnnx -File -ErrorAction SilentlyContinue
            if ($onnxFile) {
                Process-OnnxFile -OnnxFile $onnxFile -OnnxParent $onnxParent
            } else {
                Write-Host "[WARN] Quantized ONNX file $targetOnnx not found in $onnxSubfolder."
            }
        } else {
            $onnxFiles = Get-ChildItem -Path $onnxSubfolder -Filter *.onnx -File
            Write-Host "[INFO] Found $($onnxFiles.Count) .onnx files to process."
            foreach ($onnx in $onnxFiles) {
                Process-OnnxFile -OnnxFile $onnx -OnnxParent $onnxParent
            }
        }
        Write-Host "[INFO] Model preparation complete for: $ModelName"
    } else {
        Write-Host "[WARN] No ONNX subfolder found at: $onnxSubfolder. Skipping ONNX processing."
    }
}

Download-And-Prepare-Model -ModelName "onnx-community/Qwen2.5-0.5B-Instruct" -Quantization "int8"
