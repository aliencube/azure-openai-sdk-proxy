using AzureOpenAIProxy.ApiApp.Endpoints;
using AzureOpenAIProxy.ApiApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Add Azure OpenAI service.
builder.Services.AddOpenAIService();

// Add OpenAPI service.
builder.Services.AddOpenApiService();

var app = builder.Build();

// https://stackoverflow.com/questions/76962735/how-do-i-set-a-prefix-in-my-asp-net-core-7-web-api-for-all-endpoints
var basePath = "/api";
app.UsePathBase(basePath);
app.UseRouting();

// Configure the HTTP request pipeline.
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next.Invoke();
});

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseSwaggerUI(basePath);

app.AddWeatherForecast();
app.AddChatCompletions();

app.MapDefaultEndpoints();

app.Run();
