var builder = DistributedApplication.CreateBuilder(args);

var apiapp = builder.AddProject<Projects.AzureOpenAIProxy_ApiApp>("apiapp")
                    .WithExternalHttpEndpoints();

builder.AddProject<Projects.AzureOpenAIProxy_WebApp>("webapp")
       .WithExternalHttpEndpoints()
       .WithReference(apiapp);

builder.Build().Run();
