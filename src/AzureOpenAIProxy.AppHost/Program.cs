using AzureOpenAIProxy.ApiApp.Configurations;

using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage");
var table = storage.AddTables("table");

var aoai = builder.Configuration.GetSection(AoaiSettings.Name).Get<AoaiSettings>();

var apiapp = builder.AddProject<Projects.AzureOpenAIProxy_ApiApp>("apiapp")
                    .WithReference(table);

for (var i = 0; i < aoai.Instances.Count; i++)
{
    var instance = aoai.Instances[i];
    apiapp.WithEnvironment($"AOAI__Instances__{i}__Endpoint", instance.Endpoint)
          .WithEnvironment($"AOAI__Instances__{i}__ApiKey", instance.ApiKey);
}

builder.AddProject<Projects.AzureOpenAIProxy_PlaygroundApp>("playground")
       .WithEnvironment("OpenAI__Endpoint", apiapp.GetEndpoint("https"))
       .WithReference(apiapp);

builder.Build().Run();
