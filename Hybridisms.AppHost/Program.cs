var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Hybridisms_Server_WebApp>("webapp");

builder.Build().Run();
