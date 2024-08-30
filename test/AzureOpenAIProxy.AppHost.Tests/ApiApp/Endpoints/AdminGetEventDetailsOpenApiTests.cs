using System.Text.Json;

using AzureOpenAIProxy.AppHost.Tests.Fixtures;

using FluentAssertions;

using IdentityModel.Client;

namespace AzureOpenAIProxy.AppHost.Tests.ApiApp.Endpoints;

public class AdminGetEventDetailsOpenApiTests(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
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
                                         .TryGetProperty("/admin/events/{eventId}", out var property) ? property : default;
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
                                         .GetProperty("/admin/events/{eventId}")
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
                                         .GetProperty("/admin/events/{eventId}")
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
                                         .GetProperty("/admin/events/{eventId}")
                                         .GetProperty("get")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.String);
    }

    [Theory]
    [InlineData("parameters")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Array(string attribute)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events/{eventId}")
                                         .GetProperty("get")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Theory]
    [InlineData("eventId")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Path_Parameter(string name)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events/{eventId}")
                                         .GetProperty("get")
                                         .GetProperty("parameters")
                                         .EnumerateArray()
                                         .Where(p => p.GetProperty("in").GetString() == "path")
                                         .Select(p => p.GetProperty("name").ToString());
        result.Should().Contain(name);
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
                                         .GetProperty("/admin/events/{eventId}")
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
                                         .GetProperty("/admin/events/{eventId}")
                                         .GetProperty("get")
                                         .GetProperty("responses")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    public static IEnumerable<object[]> AttributeData =>
        [
            ["eventId", true, "string"],
            ["title", true, "string"],
            ["summary", true, "string"],
            ["description", false, "string"],
            ["dateStart", true, "string"],
            ["dateEnd", true, "string"],
            ["timeZone", true, "string"],
            ["isActive", true, "boolean"],
            ["organizerName", true, "string"],
            ["organizerEmail", true, "string"],
            ["coorganizerName", false, "string"],
            ["coorganizerEmail", false, "string"],
            ["maxTokenCap", true, "integer"],
            ["dailyRequestCap", true, "integer"]
        ];

    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Schemas()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("components")
                                         .TryGetProperty("schemas", out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Model()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("components")
                                         .GetProperty("schemas")
                                         .TryGetProperty("AdminEventDetails", out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Theory]
    [MemberData(nameof(AttributeData))]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Required(string attribute, bool isRequired, string type)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var isReq = isRequired;
        var typeStr = type;

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("components")
                                         .GetProperty("schemas")
                                         .GetProperty("AdminEventDetails")
                                         .TryGetStringArray("required")
                                         .ToList();
        result.Contains(attribute).Should().Be(isRequired);
    }

    [Theory]
    [MemberData(nameof(AttributeData))]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Property(string attribute, bool isRequired, string type)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var isReq = isRequired;
        var typeStr = type;

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("components")
                                         .GetProperty("schemas")
                                         .GetProperty("AdminEventDetails")
                                         .GetProperty("properties")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Theory]
    [MemberData(nameof(AttributeData))]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Type(string attribute, bool isRequired, string type)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var isReq = isRequired;
        var typeStr = type;

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("components")
                                         .GetProperty("schemas")
                                         .GetProperty("AdminEventDetails")
                                         .GetProperty("properties")
                                         .GetProperty(attribute);
        result.TryGetString("type").Should().Be(type);
    }
}
