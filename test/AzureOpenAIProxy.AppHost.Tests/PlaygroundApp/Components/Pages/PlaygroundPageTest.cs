using AzureOpenAIProxy.AppHost.Tests.Fixtures;

namespace AzureOpenAIProxy.AppHost.Tests.PlaygroundApp.Components.Pages;

public class PlaygroundPageTest(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
{
    [Fact]
    public async Task GetPlaygroundEndpointReturnSuccess()
    {
        // Arrange
        var client = host.App!.CreateHttpClient("playgroundapp");

        // Act
        var response = await client.GetAsync("/playground");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}