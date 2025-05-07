using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Turn on service discovery by default
builder.Services.AddServiceDiscovery();
builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.AddServiceDiscovery();
});

// The Blazor WASM client only supports the remote HTTP service
builder.Services.AddHttpClient<INotesService, RemoteNotesService>(static client => client.BaseAddress = new("https+http://api/"));
builder.Services.AddHttpClient<ILabelRecommendationService, RemoteLabelRecommendationService>(static client => client.BaseAddress = new("https+http://api/"));

var app = builder.Build();

await app.RunAsync();
