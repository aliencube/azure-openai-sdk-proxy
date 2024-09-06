using System.Net;

using AzureOpenAIProxy.AppHost.Tests.Fixtures;

using FluentAssertions;

namespace AzureOpenAIProxy.AppHost.Tests;

public class AppHostProgramTests(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
{
    [Theory]
    [InlineData("apiapp", "/health", HttpStatusCode.OK)]
    [InlineData("playgroundapp", "/health", HttpStatusCode.OK)]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Healthy(string resourceName, string endpoint, HttpStatusCode statusCode)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient(resourceName);
        await host.ResourceNotificationService.WaitForResourceAsync(resourceName, KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var response = await httpClient.GetAsync(endpoint);

        // Assert
        response.StatusCode.Should().Be(statusCode);
    }
}
