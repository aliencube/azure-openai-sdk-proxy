var builder = DistributedApplication.CreateBuilder(args);

var apiapp = builder.AddProject<Projects.AzureOpenAIProxy_ApiApp>("apiapp");

builder.AddProject<Projects.AzureOpenAIProxy_PlaygroundApp>("playground")
       .WithReference(apiapp);

builder.Build().Run();
