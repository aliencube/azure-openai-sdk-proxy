using System.Text.Json;
using System.Net;

using AzureOpenAIProxy.AppHost.Tests.Fixtures;

using FluentAssertions;

using System.Text;

namespace AzureOpenAIProxy.AppHost.Tests.ApiApp.Endpoints;

public class AdminCreateEventsOpenApiTests(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
{
    [Theory]
    [InlineData(@"{
        ""eventId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
        ""title"": ""string"",
        ""summary"": ""string"",
        ""description"": ""string"",
        ""dateStart"": ""2024-08-26T14:31:49.726Z"",
        ""dateEnd"": ""2024-08-26T14:31:49.726Z"",
        ""timeZone"": ""string"",
        ""isActive"": true,
        ""organizerName"": ""string"",
        ""organizerEmail"": ""string"",
        ""coorganizerName"": ""string"",
        ""coorganizerEmail"": ""string"",
        ""maxTokenCap"": 0,
        ""dailyRequestCap"": 0
    }", HttpStatusCode.OK)]
    [InlineData(@"{
        ""eventId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
        ""title"": ""string"",
        ""summary"": ""string"",
        ""dateStart"": ""2024-08-26T14:31:49.726Z"",
        ""dateEnd"": ""2024-08-26T14:31:49.726Z"",
        ""timeZone"": ""string"",
        ""isActive"": true,
        ""organizerName"": ""string"",
        ""organizerEmail"": ""string"",
        ""maxTokenCap"": 0,
        ""dailyRequestCap"": 0
    }", HttpStatusCode.OK)]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_OK(string adminEventDetails, HttpStatusCode statusCode)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");

        // Act
        var content = new StringContent(adminEventDetails, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/admin/events", content);

        // Assert
        response.StatusCode.Should().Be(statusCode);
    }
    
    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Path()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events");
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Verb()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .TryGetProperty("post", out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Theory]
    [InlineData("admin")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Tags(string tag)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .GetProperty("post")
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

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .GetProperty("post")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.String);
    }

    [Theory]
    [InlineData("requestBody")]
    [InlineData("responses")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Object(string attribute)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .GetProperty("post")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }


    [Theory]
    [InlineData("200")]
    [InlineData("401")]
    [InlineData("404")]
    [InlineData("500")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Response(string attribute)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("paths")
                                         .GetProperty("/admin/events")
                                         .GetProperty("post")
                                         .GetProperty("responses")
                                         .TryGetProperty(attribute, out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Schemas()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");

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

        // Act
        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);

        // Assert
        var result = openapi!.RootElement.GetProperty("components")
                                         .GetProperty("schemas")
                                         .TryGetProperty("AdminEventDetails", out var property) ? property : default;
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }
}
