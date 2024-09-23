using AzureOpenAIProxy.PlaygroundApp.Clients;

using Microsoft.FluentUI.AspNetCore.Components;

using App = AzureOpenAIProxy.PlaygroundApp.Components.App;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();


builder.Services.AddHttpClient<IOpenAIApiClient, OpenAIApiClient>(client =>
    client.BaseAddress = new Uri("https+http://apiapp"));

builder.Services.AddScoped<OpenAIApiClientOptions>();

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