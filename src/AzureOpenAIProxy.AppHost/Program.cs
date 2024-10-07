var builder = DistributedApplication.CreateBuilder(args);

var apiapp = builder.AddProject<Projects.AzureOpenAIProxy_ApiApp>("apiapp")
                    .WithExternalHttpEndpoints();

builder.AddProject<Projects.AzureOpenAIProxy_PlaygroundApp>("playgroundapp")
       .WithExternalHttpEndpoints()
       .WithReference(apiapp)
       .WithEnvironment("ServiceNames__Backend", apiapp.Resource.Name);

await builder.Build().RunAsync();
