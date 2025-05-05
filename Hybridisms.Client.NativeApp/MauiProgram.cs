using Hybridisms.Client.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hybridisms.Client.NativeApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        AppContext.SetSwitch("Hybridisms.SupportsRenderMode", false);

        var builder = MauiApp.CreateBuilder();

        // TODO: Add a better way to set the base address for the API service
        // This is a temporary solution to set the base address for the API service
        builder.Configuration.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
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

        // Add the /api/notes endpoint
        builder.Services.AddHttpClient<INotesService, RemoteNotesService>(static client => client.BaseAddress = new("https://api/"));

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
