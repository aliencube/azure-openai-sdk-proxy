using System.Text.Json;

using AzureOpenAIProxy.AppHost.Tests.Fixtures;

using FluentAssertions;

namespace AzureOpenAIProxy.AppHost.Tests.ApiApp.Endpoints;

public class AdminGetEventsOpenApiTests(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
{
    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Path()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .TryGetProperty("/admin/events", out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Verb()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .TryGetProperty("get", out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Theory]
    [InlineData("admin")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Tags(string tag)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .GetProperty("get")
                                         .TryGetProperty("tags", out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Array);
        result.EnumerateArray().Select(p => p.GetString()).Should().Contain(tag);
    }

    [Theory]
    [InlineData("summary")]
    [InlineData("description")]
    [InlineData("operationId")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Value(string attribute)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .GetProperty("get")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.String);
    }

    [Theory]
    [InlineData("responses")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Object(string attribute)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .GetProperty("get")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Theory]
    [InlineData("200")]
    [InlineData("401")]
    [InlineData("500")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Response(string attribute)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .GetProperty("get")
                                         .GetProperty("responses")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }
}