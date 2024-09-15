using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;
using AzureOpenAIProxy.ApiApp.Services;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace AzureOpenAIProxy.ApiApp.Tests.Services;

public class AdminEventServiceTests
{
    [Fact]
    public void Given_ServiceCollection_When_AddAdminEventService_Invoked_Then_It_Should_Contain_AdminEventService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAdminEventService();

        // Assert
        services.SingleOrDefault(p => p.ServiceType == typeof(IAdminEventService)).Should().NotBeNull();
    }

    [Fact]
    public void Given_Instance_When_CreateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventDetails = new AdminEventDetails();
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);

        // Act
        Func<Task> func = async () => await service.CreateEvent(eventDetails);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_GetEvents_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);

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
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);

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
        var eventDetails = new AdminEventDetails();
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);

        // Act
        Func<Task> func = async () => await service.UpdateEvent(eventId, eventDetails);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    // TODO: [tae0y] Add tests for other methods
    [Fact]
    public async Task Given_TimeZoneIsEmpty_When_CreateEvent_Invoked_Then_Throw_ArgumentNullException()
    {
        // Arrange
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);
        var eventDetails = createRandomEventDetails(
                new AdminEventDetails(), 
                new HashSet<string> { nameof(AdminEventDetails.TimeZone) }
            );

        // Act
        Func<Task> func = async () => await service.CreateEvent(eventDetails);

        // Assert
        await func.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task Given_TimeZoneIsNotEmpty_When_CreateEvent_Invoked_Then_PartitionKeyShould_Be_TimeZone()
    {
        // Arrange
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);
        var eventDetails = createRandomEventDetails(
                new AdminEventDetails(), 
                new HashSet<string>()
            );

        // Act
        var result = await service.CreateEvent(eventDetails);

        // Assert
        result.PartitionKey.Should().Be(eventDetails.TimeZone);
    }

    [Fact]
    public async Task Given_EventIdIsNull_When_CreateEvent_Invoked_Then_Throw_ArgumentNullException()
    {
        // Arrange
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);
        var eventDetails = createRandomEventDetails(
                new AdminEventDetails(), 
                new HashSet<string> { nameof(AdminEventDetails.EventId) }
            );

        // Act
        Func<Task> func = async () => await service.CreateEvent(eventDetails);

        // Assert
        await func.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task Given_EventIdIsNotEmpty_When_CreateEvent_Invoked_Then_RowKeyShould_Be_EventId()
    {
        // Arrange
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);
        var eventDetails = createRandomEventDetails(
                new AdminEventDetails(), 
                new HashSet<string>()
            );

        // Act
        var result = await service.CreateEvent(eventDetails);

        // Assert
        result.RowKey.Should().Be(eventDetails.RowKey);
    }

    // Helper method to create random event details
    private static AdminEventDetails createRandomEventDetails(AdminEventDetails details, HashSet<string> keepNullFields)
    {
        Random random = new Random();

        // Check if field should be skipped by checking if it exists in the hashset
        bool ShouldSkip(string fieldName)
        {
            return keepNullFields.Contains(fieldName);
        }

        // Fill in null or empty string properties with random values unless they should be skipped
        if (!ShouldSkip(nameof(details.Title)) && string.IsNullOrEmpty(details.Title))
        {
            details.Title = "Event Title " + random.Next(1000, 9999);
        }

        if (!ShouldSkip(nameof(details.Summary)) && string.IsNullOrEmpty(details.Summary))
        {
            details.Summary = "This is a summary for event " + random.Next(1000, 9999);
        }

        if (!ShouldSkip(nameof(details.Description)) && string.IsNullOrEmpty(details.Description))
        {
            details.Description = "Description for event " + random.Next(1000, 9999);
        }

        if (!ShouldSkip(nameof(details.EventId)) && details.EventId == Guid.Empty)
        {
            details.EventId = Guid.NewGuid();
        }

        if (!ShouldSkip(nameof(details.MaxTokenCap)) && details.MaxTokenCap == 0)
        {
            details.MaxTokenCap = random.Next(100, 1000); // Assign random token cap
        }

        if (!ShouldSkip(nameof(details.DailyRequestCap)) && details.DailyRequestCap == 0)
        {
            details.DailyRequestCap = random.Next(10, 100); // Assign random daily request cap
        }

        if (!ShouldSkip(nameof(details.DateStart)) && details.DateStart == DateTimeOffset.MinValue)
        {
            details.DateStart = DateTimeOffset.Now.AddDays(random.Next(-10, 10)); // Random start date
        }

        if (!ShouldSkip(nameof(details.DateEnd)) && (details.DateEnd == DateTimeOffset.MinValue || details.DateEnd <= details.DateStart))
        {
            details.DateEnd = details.DateStart.AddHours(random.Next(1, 72)); // End date should be after start date
        }

        if (!ShouldSkip(nameof(details.TimeZone)) && string.IsNullOrEmpty(details.TimeZone))
        {
            details.TimeZone = "KST"; // Default timezone
        }

        if (!ShouldSkip(nameof(details.IsActive)))
        {
            details.IsActive = true; // Default is active
        }

        if (!ShouldSkip(nameof(details.OrganizerName)) && string.IsNullOrEmpty(details.OrganizerName))
        {
            details.OrganizerName = "Organizer " + random.Next(1000, 9999);
        }

        if (!ShouldSkip(nameof(details.OrganizerEmail)) && string.IsNullOrEmpty(details.OrganizerEmail))
        {
            details.OrganizerEmail = $"organizer{random.Next(1000, 9999)}@example.com";
        }

        if (!ShouldSkip(nameof(details.CoorganizerName)) && string.IsNullOrEmpty(details.CoorganizerName))
        {
            details.CoorganizerName = "Co-organizer " + random.Next(1000, 9999);
        }

        if (!ShouldSkip(nameof(details.CoorganizerEmail)) && string.IsNullOrEmpty(details.CoorganizerEmail))
        {
            details.CoorganizerEmail = $"coorganizer{random.Next(1000, 9999)}@example.com";
        }

        return details;
    }
}
