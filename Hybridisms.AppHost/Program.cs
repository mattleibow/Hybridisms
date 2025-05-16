using Azure.Provisioning.CognitiveServices;

var builder = DistributedApplication.CreateBuilder(args);


// AI

var ai = builder.AddAzureOpenAI("ai")
    .ConfigureInfrastructure(infra =>
    {
        // South Africa does not support the default "Standard" SKU
        var resources = infra.GetProvisionableResources();
        var account = resources.OfType<CognitiveServicesAccountDeployment>().Single();
        account.Sku.Name = "GlobalStandard";
    });

ai.AddDeployment(
    name: "ai-model",
    modelName: "gpt-4o-mini",
    modelVersion: "2024-07-18");


// Data


var dataGroup = builder.AddGroup("data");

var dbDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "artifacts", "data");
var db = builder.AddSqlite("db", dbDir, "hybridisms.db")
    .InGroup(dataGroup);

var sqliteWeb = db.WithSqliteWeb(b => b.InGroup(dataGroup));

// Register the worker as a data item
var dbSeeder = builder.AddProject<Projects.Hybridisms_Server_Worker>("db-seeder")
    .InGroup(dataGroup)
    .WithReference(db)
    .WaitFor(db);

// Apps

var appsGroup = builder.AddGroup("apps");

var web = builder.AddProject<Projects.Hybridisms_Server_WebApp>("webapp")
    .InGroup(appsGroup)
    .WithReference(ai)
    .WithReference(db)
    .WaitForCompletion(dbSeeder);

if (builder.ExecutionContext.IsRunMode)
{
    builder.AddMauiProject("mobile", "Hybridisms.Client.NativeApp")
        .InGroup(appsGroup)
        .WithReference(web);

    builder.AddWasmProject("wasm", "Hybridisms.Client.WebAssembly")
        .InGroup(appsGroup)
        .WithReference(web);
}

builder.Build().Run();
