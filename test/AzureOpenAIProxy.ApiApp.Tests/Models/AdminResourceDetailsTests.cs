using System.Text.Json;

using AzureOpenAIProxy.ApiApp.Models;

using FluentAssertions;

namespace AzureOpenAIProxy.ApiApp.Tests.Models;

public class AdminResourceDetailsTests
{
    private static readonly AdminResourceDetails examplePayload = new()
    {
        ResourceId = Guid.Parse("67f410a3-c5e4-4326-a3ad-5812b9adfc06"),
        FriendlyName = "Test Resource",
        DeploymentName = "Test Deployment",
        ResourceType = ResourceType.Chat,
        Endpoint = "https://test.endpoint.com",
        ApiKey = "test-api-key",
        Region = "test-region",
        IsActive = true
    };

    private static readonly string exampleJson = """
    {
        "resourceId": "67f410a3-c5e4-4326-a3ad-5812b9adfc06",
        "friendlyName": "Test Resource",
        "deploymentName": "Test Deployment",
        "resourceType": "chat",
        "endpoint": "https://test.endpoint.com",
        "apiKey": "test-api-key",
        "region": "test-region",
        "isActive": true
    }
    """;

    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    [Fact]
    public void Given_ExamplePayload_When_Serialized_Then_It_Should_Match_Json()
    {
        // Act
        var serialised = JsonSerializer.Serialize(examplePayload, options);

        // Assert
        serialised.Should().ContainAll(
            "\"resourceId\":", "\"67f410a3-c5e4-4326-a3ad-5812b9adfc06\"", 
            "\"friendlyName\":", "\"Test Resource\"", 
            "\"deploymentName\":", "\"Test Deployment\"", 
            "\"resourceType\":", "\"chat\"", 
            "\"endpoint\":", "\"https://test.endpoint.com\"", 
            "\"apiKey\":", "\"test-api-key\"", 
            "\"region\":", "\"test-region\"", 
            "\"isActive\":", "true");
    }

    [Fact]
    public void Given_ExampleJson_When_Deserialized_Then_It_Should_Match_Object()
    {
        // Arrange & Act
        var deserialised = JsonSerializer.Deserialize<AdminResourceDetails>(exampleJson, options);

        // Assert
        deserialised.Should().BeEquivalentTo(examplePayload);
    }
}