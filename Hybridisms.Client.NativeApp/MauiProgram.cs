using Hybridisms.Client.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Hybridisms.Client.NativeApp.Services;
using Hybridisms.Client.NativeApp.Data;

namespace Hybridisms.Client.NativeApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        AppContext.SetSwitch("Hybridisms.SupportsRenderMode", false);

        var builder = MauiApp.CreateBuilder();

        builder.AddServiceDefaults();

#if DEBUG
        builder.Configuration.AddAspireApp(AspireAppSettings.Settings, "exciting-tunnel");
#endif

        // TODO: Add a better way to set these values
        builder.Configuration.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["ConnectionStrings:Hybridisms"] = "Hybridisms.db",
                ["Services:api:https:0"] = "https://localhost:7201",
            });

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // Turn on service discovery by default
        builder.Services.AddServiceDiscovery();
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddServiceDiscovery();
        });

        // Register SQLite database
        builder.Services.AddOptions<HybridismsEmbeddedDbContext.DbContextOptions>()
            .Configure(options => 
            {
                options.DatabasePath = Path.Combine(FileSystem.AppDataDirectory, builder.Configuration.GetConnectionString("Hybridisms")!);
            });
        builder.Services.AddSingleton<HybridismsEmbeddedDbContext>();

        // Register services
        builder.Services.AddSingleton<EmbeddedNotesService>();
        builder.Services.AddHttpClient<RemoteNotesService>(static client => client.BaseAddress = new("https://api/"));
        builder.Services.AddScoped<INotesService, HybridNotesService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
