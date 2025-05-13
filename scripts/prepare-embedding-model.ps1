$ModelName = "sentence-transformers/all-MiniLM-L6-v2"

Write-Host "[INFO] Starting download and preparation for model: $ModelName"

# Extract the short model name (after the last slash)
$ShortName = ($ModelName -split '/')[1]

# Download the model
$onnxParent = Join-Path $PSScriptRoot "../artifacts/models/$ShortName/Base"
Write-Host "[INFO] Downloading model to: $onnxParent"
huggingface-cli download $ModelName --local-dir $onnxParent

# Check if the ONNX subfolder exists
$onnxSubfolder = Join-Path $onnxParent "onnx"
if (!(Test-Path $onnxSubfolder)) {
    Write-Host "[WARN] No ONNX subfolder found at: $onnxSubfolder. Skipping ONNX processing."
    exit
}

$folderName = "model"

# Process ONNX files if present
Write-Host "[INFO] Found ONNX subfolder: $onnxSubfolder"

$targetDir = Join-Path (Split-Path $onnxParent -Parent) $folderName

if (-not (Test-Path $targetDir)) {
    New-Item -ItemType Directory -Path $targetDir | Out-Null
}

Write-Host "[INFO] Copying files in $onnxParent to $targetDir"
Copy-Item $onnxParent\onnx\model.onnx -Destination $targetDir
Copy-Item $onnxParent\vocab.txt -Destination $targetDir

$zipPath = Join-Path (Split-Path $targetDir -Parent) ("$folderName.zip")
Write-Host "[INFO] Zipping $targetDir to $zipPath"
if (Test-Path $zipPath) { Remove-Item $zipPath }
Compress-Archive -Path $targetDir\* -DestinationPath $zipPath

Write-Host "[INFO] Model preparation complete for: $ModelName"
