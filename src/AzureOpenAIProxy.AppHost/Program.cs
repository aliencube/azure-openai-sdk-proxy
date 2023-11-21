var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage");
var table = storage.AddTables("table");

var apiapp = builder.AddProject<Projects.AzureOpenAIProxy_ApiApp>("apiapp")
                    .WithReference(table);

builder.AddProject<Projects.AzureOpenAIProxy_PlaygroundApp>("playground")
       .WithReference(apiapp);

builder.Build().Run();
