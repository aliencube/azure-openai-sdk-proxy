var builder = DistributedApplication.CreateBuilder(args);

var apiapp = builder.AddProject<Projects.AzureOpenAIProxy_ApiApp>("apiapp")
                    .WithExternalHttpEndpoints();

var webapp = builder.AddProject<Projects.AzureOpenAIProxy_WebApp>("webapp")
                    .WithExternalHttpEndpoints()
                    .WithReference(apiapp);

await builder.Build().RunAsync();
