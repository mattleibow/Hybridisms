using Hybridisms.Server.Data;
using Hybridisms.Server.Worker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<HybridismsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("db")?.Replace("Extensions=[]", "")));

builder.Services.AddHostedService<Worker>();

builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var host = builder.Build();

host.Run();
