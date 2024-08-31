using AzureOpenAIProxy.ApiApp.Endpoints;
using AzureOpenAIProxy.ApiApp.Extensions;

using Azure.Data.Tables;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add KeyVault service
builder.Services.AddKeyVaultService();

// Add Azure OpenAI service.
builder.Services.AddOpenAIService();

// Add OpenAPI service
builder.Services.AddOpenApiService();

// Add Table service
builder.Services.AddSingleton<TableServiceClient>(sp =>
{
    //TODO: StorageAccount를 bicep으로 배포하기 (지금은 az 명령어로 수동생성함)
    //TODO: connection string을 apphost에서 넘겨받기
    //TODO: connection string을 StorageAccount 배포중에 넘겨받아서 안전하게 전달/관리하기
    // 그 전까지는 appsettings.json에서 connection string을 가져오자
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["Azure:StorageAccount:ConnectionString"]; //TODO: 이게맞나? 리팩토링 고민

    return new TableServiceClient(connectionString);
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

// Proxy endpoints
app.AddChatCompletions();

// Playground endpoints
app.AddListEvents();

// Admin endpoints
app.AddNewAdminEvent();
app.AddListAdminEvents();
app.AddGetAdminEvent();
app.AddUpdateAdminEvent();

await app.RunAsync();