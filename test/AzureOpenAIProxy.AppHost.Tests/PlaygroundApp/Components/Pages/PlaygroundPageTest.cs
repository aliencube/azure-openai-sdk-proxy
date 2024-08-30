using AzureOpenAIProxy.AppHost.Tests.Fixtures;

namespace AzureOpenAIProxy.AppHost.Tests.PlaygroundApp.Components.Pages;

public class PlaygroundPageTest(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
{
    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_OK()
    {
        // Arrange
        var httpClient = host.App!.CreateHttpClient("playgroundapp");
        await host.ResourceNotificationService.WaitForResourceAsync("playgroundapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var response = await httpClient.GetAsync("/playground");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}