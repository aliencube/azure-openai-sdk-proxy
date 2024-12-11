using System.Net;

using AzureOpenAIProxy.AppHost.Tests.Fixtures;

using FluentAssertions;

namespace AzureOpenAIProxy.AppHost.Tests.PlaygroundApp.Pages;

public class AdminUpdateEventPageTests(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
{
    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_OK()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("playgroundapp");
        await host.ResourceNotificationService.WaitForResourceAsync("playgroundapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var eventId = Guid.NewGuid();
        var expectedUrl = $"/admin/events/edit/{eventId}";

        // Act
        var response = await httpClient.GetAsync(expectedUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_CSS_Elements(string expected)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("playgroundapp");
        await host.ResourceNotificationService.WaitForResourceAsync("playgroundapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var eventId = Guid.NewGuid();
        var expectedUrl = $"/admin/events/edit/{eventId}";

        // Act
        var html = await httpClient.GetStringAsync(expectedUrl);

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

        var eventId = Guid.NewGuid();
        var expectedUrl = $"/admin/events/edit/{eventId}";

        // Act
        var html = await httpClient.GetStringAsync(expectedUrl);

        // Assert
        html.Should().Contain(expected);
    }

    [Theory]
    [InlineData("<div class=\"fluent-tooltip-provider\" style=\"position: fixed; z-index: 9999;\"></div>")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_HTML_Elements(string expected)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("playgroundapp");
        await host.ResourceNotificationService.WaitForResourceAsync("playgroundapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var eventId = Guid.NewGuid();
        var expectedUrl = $"/admin/events/edit/{eventId}";

        // Act
        var html = await httpClient.GetStringAsync(expectedUrl);

        // Assert
        html.Should().Contain(expected);
    }
}