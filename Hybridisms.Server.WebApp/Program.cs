using Hybridisms.Shared.Services;
using Hybridisms.Server.Data;
using Hybridisms.Server.Services;
using Hybridisms.Server.WebApp.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add EF Core DbContext with SQLite
builder.Services.AddDbContext<HybridismsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("db")));

// Add controller services
builder.Services.AddControllers();

// Register intelligence service
builder.Services.AddScoped<IIntelligenceService, AdvancedIntelligenceService>();

// The Blazor server app only supports the local database service
builder.Services.AddScoped<INotesService, DbNotesService>();

// Register Azure OpenAI client
builder.AddAzureOpenAIClient("ai")
    .AddChatClient("ai-model");

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
    .AddAdditionalAssemblies(typeof(Hybridisms.Hosting._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(Hybridisms.Shared._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(Hybridisms.Client.WebAssembly._Imports).Assembly);

// Map controller endpoints
app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
