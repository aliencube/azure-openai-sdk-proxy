using AzureOpenAIProxy.PlaygroundApp.Clients;
using AzureOpenAIProxy.PlaygroundApp.Components;
using AzureOpenAIProxy.PlaygroundApp.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<OpenAISettings>(p => p.GetService<IConfiguration>().GetSection(OpenAISettings.Name).Get<OpenAISettings>());
builder.Services.AddHttpClient<IApiAppClient, ApiAppClient>(client =>
{
    client.BaseAddress = new Uri("https://apiapp");
});

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

app.Run();
