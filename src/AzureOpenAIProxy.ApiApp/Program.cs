using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Filters;
using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddAzureTableService("table");

// Add services to the container.
builder.Services.AddSingleton<AoaiSettings>(p => p.GetService<IConfiguration>().GetSection(AoaiSettings.Name).Get<AoaiSettings>());
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<IManagementService, ManagementService>();
builder.Services.AddKeyedScoped<IAuthService<AccessCodeRecord>, UserAuthService>("accesscode");
builder.Services.AddKeyedScoped<IAuthService<EventRecord>, ManagementAuthService>("management");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Azure OpenAI Proxy Service",
            Description = "Providing a proxy service to Azure OpenAI",
        });
    options.AddSecurityDefinition(
        "apiKey",
        new OpenApiSecurityScheme()
        {
            Name = "api-key",
            Type = SecuritySchemeType.ApiKey,
            Description = "API key needed to access the endpoints.",
            In = ParameterLocation.Header,
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "apiKey" }
            },
            new string[] {}
        }
    });
    options.OperationFilter<OpenApiParameterIgnoreFilter>();
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next.Invoke();
});

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
