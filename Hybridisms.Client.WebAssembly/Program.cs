using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

#if DEBUG
builder.Configuration.AddInMemoryCollection(AspireAppSettings.Settings);
#endif

// Add Aspire/ServiceDefaults configuration
builder.AddServiceDefaults();

// The Blazor WASM client only supports the remote HTTP service
builder.Services.AddHttpClient<INotesService, RemoteNotesService>(static client => client.BaseAddress = new("https+http://webapp/"));
builder.Services.AddHttpClient<IIntelligenceService, RemoteIntelligenceService>(static client => client.BaseAddress = new("https+http://webapp/"));

var app = builder.Build();

await app.RunAsync();
