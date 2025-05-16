using Hybridisms.Shared.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Hybridisms.Client.Native.Data;
using Hybridisms.Client.Native.Services;
using Hybridisms.Client.NativeApp.Services;

namespace Hybridisms.Client.NativeApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Disable hybrid render mode for this app
        AppContext.SetSwitch("Hybridisms.SupportsRenderMode", false);

        var builder = MauiApp.CreateBuilder();

#if DEBUG
        builder.Configuration.AddDevTunnelsInMemoryCollection(AspireAppSettings.Settings, null); // TODO: set up dev tunnels
#endif

        // Add Aspire/ServiceDefaults configuration
        builder.AddServiceDefaults();

        // Register the main Maui app and configure fonts
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Add Blazor WebView for hybrid UI
        builder.Services.AddMauiBlazorWebView();

        // Register SQLite database context for local data storage
        builder.Services.AddOptions<HybridismsEmbeddedDbContext.DbContextOptions>()
            .Configure(options =>
            {
                options.DatabasePath = Path.Combine(FileSystem.AppDataDirectory, "hybridisms.db");
            });
        builder.Services.AddSingleton<HybridismsEmbeddedDbContext>();

        // Register notes services
        builder.Services.AddHttpClient<RemoteNotesService>(static client => client.BaseAddress = new("https://webapp/"));
        builder.Services.AddSingleton<EmbeddedNotesService>();

        // Register ONNX clients for local AI processing
        builder.Services.AddOptions<OnnxEmbeddingClient.EmbeddingClientOptions>()
            .Configure(options => 
            {
                options.BundledPath = "Models/miniml_model.zip";
                options.ExtractedPath = Path.Combine(FileSystem.AppDataDirectory, "Models", "embedding_model");
            });
        builder.Services.AddSingleton<OnnxEmbeddingClient>();
        builder.Services.AddOptions<OnnxChatClient.ChatClientOptions>()
            .Configure(options => 
            {
                options.BundledPath = "Models/qwen2_model.zip";
                options.ExtractedPath = Path.Combine(FileSystem.AppDataDirectory, "Models", "chat_model");
            });
        builder.Services.AddSingleton<OnnxChatClient>();

        // Register intelligence services for AI capabilities
        builder.Services.AddHttpClient<RemoteIntelligenceService>(static client => client.BaseAddress = new("https://webapp/"));
        builder.Services.AddSingleton<EmbeddedIntelligenceService>();

        // Register the hybrid services that we will use
        builder.Services.AddScoped<INotesService, HybridNotesService>();
        builder.Services.AddScoped<IIntelligenceService, HybridIntelligenceService>();

        // Register MauiAppFileProvider as a singleton for IAppFileProvider
        builder.Services.AddSingleton<IAppFileProvider, MauiAppFileProvider>();

#if DEBUG
        // Enable developer tools and debug logging in debug builds
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
