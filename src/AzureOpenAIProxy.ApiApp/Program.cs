using System.Reflection;

using AzureOpenAIProxy.ApiApp.Endpoints;
using AzureOpenAIProxy.ApiApp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Azure OpenAI service.
builder.Services.AddOpenAIService();

// Add OpenAPI service
builder.Services.AddOpenApiService();

var app = builder.Build();

app.MapDefaultEndpoints();

// https://stackoverflow.com/questions/76962735/how-do-i-set-a-prefix-in-my-asp-net-core-7-web-api-for-all-endpoints
var basePath = "/api";
app.UsePathBase(basePath);
app.UseRouting();

// Configure the HTTP request pipeline.
// Use Swagger UI
app.UseSwaggerUI(basePath);
app.UseSwagger();

// Enable buffering
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next.Invoke();
});

app.UseHttpsRedirection();

app.AddWeatherForecast();
app.AddChatCompletions();
app.AddEventEndpoint();

await app.RunAsync();
