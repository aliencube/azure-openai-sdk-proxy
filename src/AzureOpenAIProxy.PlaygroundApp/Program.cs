using Azure;
using Azure.AI.OpenAI;

using AzureOpenAIProxy.PlaygroundApp.Clients;

using Microsoft.FluentUI.AspNetCore.Components;

using App = AzureOpenAIProxy.PlaygroundApp.Components.App;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddScoped<IOpenAIApiClient, OpenAIApiClient>();

builder.Services.AddSingleton(_ => new AzureOpenAIClient(
    new Uri("https+http://apiapp"),
    new AzureKeyCredential("abcdef")
));

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