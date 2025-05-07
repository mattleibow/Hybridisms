using Hybridisms.Client.Shared.Services;
using Hybridisms.Server.WebApp.Services;
using Hybridisms.Server.WebApp.Data;
using Microsoft.EntityFrameworkCore;
using Hybridisms.Server.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add EF Core DbContext with SQLite
builder.Services.AddDbContext<HybridismsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Hybridisms")));

// Add controller services
builder.Services.AddControllers();

// Register label recommendation service
builder.Services.AddScoped<ILabelRecommendationService, DbLabelRecommendationService>();

// The Blazor server app only supports the local database service
builder.Services.AddScoped<INotesService, DbNotesService>();

// Turn on service discovery by default
builder.Services.AddServiceDiscovery();
builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.AddServiceDiscovery();
});

var app = builder.Build();

// Apply EF Core migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HybridismsDbContext>();

    await db.Database.EnsureCreatedAsync();
    await db.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Hybridisms.Server.Shared._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(Hybridisms.Client.Shared._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(Hybridisms.Client.WebAssembly._Imports).Assembly);

// Map controller endpoints
app.MapControllers();

app.Run();
