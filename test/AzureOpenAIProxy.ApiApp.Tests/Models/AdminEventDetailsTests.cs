using System.Text.Json;

using AzureOpenAIProxy.ApiApp.Models;

using FluentAssertions;

namespace AzureOpenAIProxy.ApiApp.Tests.Models;

public class AdminEventDetailsTests
{
    private static readonly AdminEventDetails examplePayload = new()
    {
        EventId = Guid.Parse("67f410a3-c5e4-4326-a3ad-5812b9adfc06"),
        Title = "Test Title",
        Summary = "Test Summary",
        Description = "Test Description",
        DateStart = DateTimeOffset.Parse("2024-12-01T12:34:56+00:00"),
        DateEnd = DateTimeOffset.Parse("2024-12-02T12:34:56+00:00"),
        TimeZone = "Asia/Seoul",
        IsActive = true,
        OrganizerName = "Test Organizer",
        OrganizerEmail = "organiser@testemail.com",
        CoorganizerName = "Test Coorganizer",
        CoorganizerEmail = "coorganiser@testemail.com",
        MaxTokenCap = 1000,
        DailyRequestCap = 4000,
    };

    private static readonly string exampleJson = """
    {
        "eventId": "67f410a3-c5e4-4326-a3ad-5812b9adfc06",
        "title": "Test Title",
        "summary": "Test Summary",
        "description": "Test Description",
        "dateStart": "2024-12-01T12:34:56+00:00",
        "dateEnd": "2024-12-02T12:34:56+00:00",
        "timeZone": "Asia/Seoul",
        "isActive": true,
        "organizerName": "Test Organizer",
        "organizerEmail": "organiser@testemail.com",
        "coorganizerName": "Test Coorganizer",
        "coorganizerEmail": "coorganiser@testemail.com",
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
            "\"description\":", "\"Test Description\"",
            "\"dateStart\":", "\"2024-12-01T12:34:56+00:00\"",
            "\"dateEnd\":", "\"2024-12-02T12:34:56+00:00\"",
            "\"timeZone\":", "\"Asia/Seoul\"",
            "\"isActive\":", "true",
            "\"organizerName\":", "\"Test Organizer\"",
            "\"organizerEmail\":", "\"organiser@testemail.com\"",
            "\"coorganizerName\":", "\"Test Coorganizer\"",
            "\"coorganizerEmail\":", "\"coorganiser@testemail.com\"",
            "\"maxTokenCap\":", "1000",
            "\"dailyRequestCap\":", "4000");
    }

    [Fact]
    public void Given_ExampleJson_When_Deserialized_Then_It_Should_Match_Object()
    {
        // Arrange & Act
        var deserialised = JsonSerializer.Deserialize<AdminEventDetails>(exampleJson, options);

        // Assert
        deserialised.Should().BeEquivalentTo(examplePayload);
    }
}