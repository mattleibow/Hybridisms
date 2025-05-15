using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hybridisms.Client.Native.Services;

public abstract class OnnxModelClient<TOptions>(IAppFileProvider fileProvider, IOptions<TOptions> options, ILogger<OnnxModelClient<TOptions>>? logger) : object
    where TOptions : OnnxModelClient<TOptions>.OnnxModelClientOptions
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
