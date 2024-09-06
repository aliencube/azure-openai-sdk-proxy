using Microsoft.Extensions.DependencyInjection;

namespace AzureOpenAIProxy.AppHost.Tests.Fixtures;

/// <summary>
/// This class instance is automatically created before run test.
/// To use, inherit IClassFixture<AspireHostFixture> and inject on constructor.
/// </summary>
public class AspireAppHostFixture : IAsyncLifetime
{
    public DistributedApplication? App { get; private set; }
    public ResourceNotificationService ResourceNotificationService => this.App!.Services.GetRequiredService<ResourceNotificationService>();

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AzureOpenAIProxy_AppHost>();
        this.App = await appHost.BuildAsync();
        await App.StartAsync();

        Assert.NotNull(App);
    }

    public async Task DisposeAsync()
    {
        if (App == null) return;

        await App.StopAsync();
        await App.DisposeAsync();
    }
}
