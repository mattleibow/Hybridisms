using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using static Hybridisms.Client.Native.Services.OnnxModelClient;

namespace Hybridisms.Client.Native.Services;

// TODO: AI - [B] Embedded ONNX model client base
/// <summary>
/// OnnxModelClient is a base class for ONNX model clients and provides the functionality
/// to extract and manage ONNX models.
/// 
/// The main purpose of this class is to ensure that the ONNX model is extracted from
/// the app package and available for use.
/// </summary>
public abstract class OnnxModelClient(IAppFileProvider fileProvider, IOptions<OnnxModelClientOptions> options, ILogger<OnnxModelClient>? logger)
{
    private readonly SemaphoreSlim isModelReadyLock = new(1, 1);

    protected string ExtractedModelPath => Path.Combine(options.Value.ExtractedPath, "model.onnx");

    protected async Task EnsureModelExtractedAsync()
    {
        if (File.Exists(ExtractedModelPath))
            return;

        await isModelReadyLock.WaitAsync();

        if (File.Exists(ExtractedModelPath))
            return;

        try
        {
            await Task.Run(UnpackModelAsync);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Failed to unpack model from {BundledPath} to {ExtractedPath}", options.Value.BundledPath, options.Value.ExtractedPath);
        }
        finally
        {
            isModelReadyLock.Release();
        }
    }

    private async Task UnpackModelAsync()
    {
        logger?.LogInformation("Unpacking model from {BundledPath} to {ExtractedPath}", options.Value.BundledPath, options.Value.ExtractedPath);

        using var source = await fileProvider.OpenAppPackageFileAsync(options.Value.BundledPath);
        using var archive = new ZipArchive(source, ZipArchiveMode.Read);
        Directory.CreateDirectory(options.Value.ExtractedPath);
        archive.ExtractToDirectory(options.Value.ExtractedPath, true);

        logger?.LogInformation("Model unpacked successfully.");
    }

    public class OnnxModelClientOptions
    {
        public required string BundledPath { get; set; }
        public required string ExtractedPath { get; set; }
    }
}
