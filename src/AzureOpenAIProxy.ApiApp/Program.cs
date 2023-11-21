using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddAzureTableService("table");

// Add services to the container.
builder.Services.AddSingleton<AoaiSettings>(p => p.GetService<IConfiguration>().GetSection(AoaiSettings.Name).Get<AoaiSettings>());
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();
builder.Services.AddKeyedScoped<IAuthService, UserAuthService>("accesscode");
builder.Services.AddKeyedScoped<IAuthService, ManagementAuthService>("management");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
