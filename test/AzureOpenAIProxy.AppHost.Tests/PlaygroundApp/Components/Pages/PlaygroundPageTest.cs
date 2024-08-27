using AzureOpenAIProxy.AppHost.Tests.Fixtures;

namespace AzureOpenAIProxy.AppHost.Tests.PlaygroundApp.Components.Pages;

public class PlaygroundPageTest(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
{
    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_OK()
    {
        // Arrange
        var client = host.App!.CreateHttpClient("playgroundapp");

        // Act
        var response = await client.GetAsync("/playground");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}