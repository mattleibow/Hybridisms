using Azure.Provisioning.CognitiveServices;

var builder = DistributedApplication.CreateBuilder(args);

var ai = builder.AddAzureOpenAI("ai")
     .ConfigureInfrastructure(infra =>
     {
         // South Aftrica does not support the default "Standard" SKU
         var resources = infra.GetProvisionableResources();
         var account = resources.OfType<CognitiveServicesAccountDeployment>().Single();
         account.Sku.Name = "GlobalStandard";
     })
    .AddDeployment("ai-model", "gpt-4o-mini", "2024-07-18");

builder.AddProject<Projects.Hybridisms_Server_WebApp>("webapp")
    .WithReference(ai);

if (builder.ExecutionContext.IsRunMode)
{
    builder.AddMobileProject("mobile", "../Hybridisms.Client.NativeApp")
        .WithReference(ai);
}

builder.Build().Run();
