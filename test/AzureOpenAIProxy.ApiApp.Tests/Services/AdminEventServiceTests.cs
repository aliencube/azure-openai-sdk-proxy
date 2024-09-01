using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Services;

using FluentAssertions;

namespace AzureOpenAIProxy.ApiApp.Tests.Services;

public class AdminEventServiceTests
{
    [Fact]
    public void Given_Instance_When_CreateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventDetails = new AdminEventDetails()
        {
            EventId = Guid.NewGuid(),
            Title = "Event",
            Summary = "Event summary",
            Description = "Event description",
            DateStart = DateTimeOffset.UtcNow,
            DateEnd = DateTimeOffset.UtcNow,
            TimeZone = "UTC",
            IsActive = true,
            OrganizerName = "Organiser",
            OrganizerEmail = "organiser@email.com",
            MaxTokenCap = 100,
            DailyRequestCap = 1000,
        };
        var service = new AdminEventService();

        // Act
        Func<Task> func = async () => await service.CreateEvent(eventDetails);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_GetEvents_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var service = new AdminEventService();

        // Act
        Func<Task> func = async () => await service.GetEvents();

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_GetEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var service = new AdminEventService();

        // Act
        Func<Task> func = async () => await service.GetEvent(eventId);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_UpdateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDetails = new AdminEventDetails()
        {
            EventId = Guid.NewGuid(),
            Title = "Event",
            Summary = "Event summary",
            Description = "Event description",
            DateStart = DateTimeOffset.UtcNow,
            DateEnd = DateTimeOffset.UtcNow,
            TimeZone = "UTC",
            IsActive = true,
            OrganizerName = "Organiser",
            OrganizerEmail = "organiser@email.com",
            MaxTokenCap = 100,
            DailyRequestCap = 1000,
        };
        var service = new AdminEventService();

        // Act
        Func<Task> func = async () => await service.UpdateEvent(eventId, eventDetails);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }
}
