using System.Net;

using FluentAssertions;

namespace AzureOpenAIProxy.Tests;

public class AppHostProgramTests
{
    [Theory]
    [InlineData("apiapp", "/health", HttpStatusCode.OK)]
    [InlineData("playgroundapp", "/health", HttpStatusCode.OK)]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Healthy(string resourceName, string endpoint, HttpStatusCode statusCode)
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AzureOpenAIProxy_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient(resourceName);
        var response = await httpClient.GetAsync(endpoint);

        // Assert
        response.StatusCode.Should().Be(statusCode);
    }
}
