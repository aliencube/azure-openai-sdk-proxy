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

    [Theory]
    [InlineData(@"{
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
    }", HttpStatusCode.BadRequest)]
    [InlineData(@"{}", HttpStatusCode.BadRequest)]
    public async Task Given_Resource_When_Invoked_Endpoint_Then_It_Should_Return_Bad_Request(string adminEventDetails, HttpStatusCode statusCode)
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");

        // Act
        var content = new StringContent(adminEventDetails, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/admin/events", content);

        // Assert
        response.StatusCode.Should().Be(statusCode);
    }
    
}
