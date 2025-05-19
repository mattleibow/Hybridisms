using Hybridisms.Server.Data;
using Hybridisms.Server.Services;
using Hybridisms.Server.WebApp.Components;
using Hybridisms.Shared.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


// TODO: AI - [D] Cloud AI service registration

// Register Azure OpenAI client
builder.AddAzureOpenAIClient("ai")
    .AddChatClient("ai-model");

// Register intelligence service
builder.Services.AddScoped<IIntelligenceService, AdvancedIntelligenceService>();

// The blazor service can acces the DB directly
builder.Services.AddScoped<INotesService, DbNotesService>();


// TODO: Data - [D] Cloud database registration

// Add EF Core DbContext with SQLite
builder.Services.AddDbContext<HybridismsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("db")?.Replace("Extensions=[]", "")));


// Add controller services
builder.Services.AddControllers();

var app = builder.Build();

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
