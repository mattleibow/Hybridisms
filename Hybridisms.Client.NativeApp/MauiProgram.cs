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
        builder.Configuration.AddDevTunnelsInMemoryCollection(AspireAppSettings.Settings, null);
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


        // TODO: AI - [D] Embedded/remote/hybrid AI access
        {
            // 1. Register ONNX clients for local AI processing
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

            // 2. Register the local AI service for direct AI access
            builder.Services.AddSingleton<EmbeddedIntelligenceService>();

            // 3. Register the remote REST endpoint for remote/cloud AI access
            builder.Services.AddHttpClient<RemoteIntelligenceService>(static client => client.BaseAddress = new("https://webapp/"));

            // 4. Register the hybrid services that will manage the AI access
            builder.Services.AddScoped<IIntelligenceService, HybridIntelligenceService>();
        }


        // TODO: Data - [D] Embedded/remote/hybrid data access
        {
            // 1. Register SQLite database context for local data storage
            builder.Services.AddOptions<HybridismsEmbeddedDbContext.DbContextOptions>()
                .Configure(options =>
                {
                    options.DatabasePath = Path.Combine(FileSystem.Current.AppDataDirectory, "data", "hybridisms.db");
                });
            builder.Services.AddSingleton<HybridismsEmbeddedDbContext>();

            // 2. Register the local data service for direct data access
            builder.Services.AddSingleton<EmbeddedNotesService>();

            // 3. Register the remote REST endpoint for remote/cloud data access
            builder.Services.AddHttpClient<RemoteNotesService>(static client => client.BaseAddress = new("https://webapp/"));

            // 4. Register the hybrid services that will manage the data access
            builder.Services.AddScoped<INotesService, HybridNotesService>();
        }


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
