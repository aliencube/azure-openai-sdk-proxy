using System.Text.Json;

using AzureOpenAIProxy.AppHost.Tests.Fixtures;

using FluentAssertions;

using IdentityModel.Client;

namespace AzureOpenAIProxy.AppHost.Tests.ApiApp.Endpoints;

public class AdminCreateResourcesOpenApiTests(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
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
                                         .GetProperty("/admin/resources");
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
                                         .GetProperty("/admin/resources")
                                         .GetProperty("post");
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
                                         .GetProperty("/admin/resources")
                                         .GetProperty("post")
                                         .GetProperty("tags");
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
                                         .GetProperty("/admin/resources")
                                         .GetProperty("post")
                                         .GetProperty(attribute);
        result.ValueKind.Should().Be(JsonValueKind.String);
    }

    [Theory]
    [InlineData("requestBody")]
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
                                         .GetProperty("/admin/resources")
                                         .GetProperty("post")
                                         .GetProperty(attribute);
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Theory]
    [InlineData("200")]
    [InlineData("400")]
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
                                         .GetProperty("/admin/resources")
                                         .GetProperty("post")
                                         .GetProperty("responses")
                                         .GetProperty(attribute);
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

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
                                         .GetProperty("schemas");
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
                                         .GetProperty("AdminResourceDetails");
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }
    
    [Theory]
    [InlineData("resourceId", true)]
    [InlineData("friendlyName", true)]
    [InlineData("deploymentName", true)]
    [InlineData("resourceType", true)]
    [InlineData("endpoint", true)]
    [InlineData("apiKey", true)]
    [InlineData("region", true)]
    [InlineData("isActive", true)]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Required(string attribute, bool isRequired)
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
                                         .GetProperty("AdminResourceDetails")
                                         .TryGetStringArray("required")
                                         .ToList();
        result.Contains(attribute).Should().Be(isRequired);
    }

    [Theory]
    [InlineData("resourceId")]
    [InlineData("friendlyName")]
    [InlineData("deploymentName")]
    [InlineData("resourceType")]
    [InlineData("endpoint")]
    [InlineData("apiKey")]
    [InlineData("region")]
    [InlineData("isActive")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Property(string attribute)
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
                                         .GetProperty("AdminResourceDetails")
                                         .GetProperty("properties")
                                         .GetProperty(attribute);
        result.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Theory]
    [InlineData("resourceId", "string")]
    [InlineData("friendlyName", "string")]
    [InlineData("deploymentName", "string")]
    [InlineData("resourceType", "string")]
    [InlineData("endpoint", "string")]
    [InlineData("apiKey", "string")]
    [InlineData("region", "string")]
    [InlineData("isActive", "boolean")]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Type(string attribute, string type)
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
                                         .GetProperty("AdminResourceDetails")
                                         .GetProperty("properties")
                                         .GetProperty(attribute);

        if (!result.TryGetProperty("type", out var typeProperty))
        {
            var refPath = result.TryGetString("$ref").TrimStart('#', '/').Split('/');
            var refSchema = openapi.RootElement;

            foreach (var part in refPath)
            {
                refSchema = refSchema.GetProperty(part);
            }

            typeProperty = refSchema.GetProperty("type");
        }
        
        typeProperty.GetString().Should().Be(type);
    }

    [Fact]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Validate_ResourceType_As_Enum()
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
                                         .GetProperty("AdminResourceDetails")
                                         .GetProperty("properties")
                                         .GetProperty("resourceType");

        var refPath = result.TryGetString("$ref").TrimStart('#', '/').Split('/');
        var refSchema = openapi.RootElement;

        foreach (var part in refPath)
        {
            refSchema = refSchema.GetProperty(part);
        }

        var enumValues = refSchema.GetProperty("enum")
                                  .EnumerateArray()
                                  .Select(p => p.GetString())
                                  .ToList();

        enumValues.Should().BeEquivalentTo(["none", "chat", "image"]);
    }
}