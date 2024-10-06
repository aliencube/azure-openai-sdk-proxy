using AzureOpenAIProxy.PlaygroundApp.Clients;
using AzureOpenAIProxy.PlaygroundApp.Configurations;

using Microsoft.FluentUI.AspNetCore.Components;

using App = AzureOpenAIProxy.PlaygroundApp.Components.App;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddSingleton<ServicesSettings>(sp =>
{
    var configuration = sp.GetService<IConfiguration>()
        ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

    var settings = configuration.GetSection(ServicesSettings.Name).Get<ServicesSettings>()
        ?? throw new InvalidOperationException($"{nameof(ServicesSettings)} could not be retrieved from the configuration.");

    return settings!;
});

builder.Services.AddSingleton<ServiceNamesSettings>(sp =>
{
    var configuration = sp.GetService<IConfiguration>()
        ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

    var settings = configuration.GetSection(ServiceNamesSettings.Name).Get<ServiceNamesSettings>()
        ?? throw new InvalidOperationException($"{nameof(ServiceNamesSettings)} could not be retrieved from the configuration.");

    return settings!;
});

builder.Services.AddScoped<IOpenAIApiClient, OpenAIApiClient>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();