using System.Text.Json;

using FluentAssertions;

namespace AzureOpenAIProxy.ApiApp.Tests.Models;

public class EventDetailsTests
{
    private static readonly EventDetails examplePayload = new()
    {
        EventId = Guid.Parse("67f410a3-c5e4-4326-a3ad-5812b9adfc06"),
        Title = "Test Title",
        Summary = "Test Summary",
        MaxTokenCap = 1000,
        DailyRequestCap = 4000,
    };

    private static readonly string exampleJson = """
    {
        "eventId": "67f410a3-c5e4-4326-a3ad-5812b9adfc06",
        "title": "Test Title",
        "summary": "Test Summary",
        "maxTokenCap": 1000,
        "dailyRequestCap": 4000
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
            "\"eventId\":", "\"67f410a3-c5e4-4326-a3ad-5812b9adfc06\"",
            "\"title\":", "\"Test Title\"",
            "\"summary\":", "\"Test Summary\"",
            "\"maxTokenCap\":", "1000",
            "\"dailyRequestCap\":", "4000");
    }

    [Fact]
    public void Given_ExampleJson_When_Deserialized_Then_It_Should_Match_Object()
    {
        // Arrange & Act
        var deserialised = JsonSerializer.Deserialize<EventDetails>(exampleJson, options);

        // Assert
        deserialised.Should().BeEquivalentTo(examplePayload);
    }
}