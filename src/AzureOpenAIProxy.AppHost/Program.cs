var builder = DistributedApplication.CreateBuilder(args);

var apiapp = builder.AddProject<Projects.AzureOpenAIProxy_ApiApp>("apiapp")
                    .WithExternalHttpEndpoints();

builder.AddProject<Projects.AzureOpenAIProxy_PlaygroundApp>("playgroundapp")
       .WithExternalHttpEndpoints()
       .WithReference(apiapp);

await builder.Build().RunAsync();
