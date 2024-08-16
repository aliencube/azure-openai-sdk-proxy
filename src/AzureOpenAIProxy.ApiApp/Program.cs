using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Endpoints;
using AzureOpenAIProxy.ApiApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Azure OpenAI service.
builder.Services.AddOpenAIService();

// Add OpenAPI service
builder.Services.AddOpenApiService();

// Add KeyVault service
builder.Services.AddScoped<SecretClient>(sp =>
{
    var configuration = sp.GetService<IConfiguration>()
        ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

    var settings = configuration.GetSection(AzureSettings.Name).GetSection(KeyVaultSettings.Name).Get<KeyVaultSettings>()
        ?? throw new InvalidOperationException($"{nameof(KeyVaultSettings)} could not be retrieved from the configuration.");

    var client = new SecretClient(new Uri(settings.VaultUri!), new DefaultAzureCredential());

    return client;
});

var app = builder.Build();

app.MapDefaultEndpoints();

// https://stackoverflow.com/questions/76962735/how-do-i-set-a-prefix-in-my-asp-net-core-7-web-api-for-all-endpoints
var basePath = "/api";
app.UsePathBase(basePath);
app.UseRouting();

// Configure the HTTP request pipeline.
// Use Swagger UI
app.UseSwaggerUI(basePath);

// Enable buffering
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next.Invoke();
});

app.UseHttpsRedirection();

app.AddWeatherForecast();
app.AddChatCompletions();

await app.RunAsync();
