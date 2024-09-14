using AzureOpenAIProxy.ApiApp.Endpoints;
using AzureOpenAIProxy.ApiApp.Extensions;
using AzureOpenAIProxy.ApiApp.Repositories;
using AzureOpenAIProxy.ApiApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add KeyVault service
builder.Services.AddKeyVaultService();

// Add Azure OpenAI service.
builder.Services.AddOpenAIService();

// Add OpenAPI service
builder.Services.AddOpenApiService();

// Add TableStorageClient
builder.Services.AddTableStorageService();

// Add admin services
builder.Services.AddAdminEventService();

// Add admin repositories
builder.Services.AddAdminEventRepository();

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

// Proxy endpoints
app.AddChatCompletions();

// Playground endpoints
app.AddListEvents();
app.AddListDeploymentModels();

// Admin endpoints
app.AddNewAdminEvent();
app.AddListAdminEvents();
app.AddGetAdminEvent();
app.AddUpdateAdminEvent();

await app.RunAsync();
