using AzureOpenAIProxy.AppHost.Tests.Fixtures;

using FluentAssertions;

namespace AzureOpenAIProxy.AppHost.Tests.PlaygroundApp.Pages;

public class PlaygroundPageTests(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
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
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_CSS_Elements(string expected)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("playgroundapp");
        await host.ResourceNotificationService.WaitForResourceAsync("playgroundapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var html = await httpClient.GetStringAsync("/playground");

        // Assert
        html.Should().Contain(expected);
    }

    [Theory]
    [InlineData("_content/Microsoft.FluentUI.AspNetCore.Components/Microsoft.FluentUI.AspNetCore.Components.lib.module.js")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_JavaScript_Elements(string expected)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("playgroundapp");
        await host.ResourceNotificationService.WaitForResourceAsync("playgroundapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var html = await httpClient.GetStringAsync("/playground");

        // Assert
        html.Should().Contain(expected);
    }

    [Theory]
    [InlineData("<div class=\"fluent-tooltip-provider\" style=\"display: fixed;\"></div>")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_HTML_Elements(string expected)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("playgroundapp");
        await host.ResourceNotificationService.WaitForResourceAsync("playgroundapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var html = await httpClient.GetStringAsync("/playground");

        // Assert
        html.Should().Contain(expected);
    }
}