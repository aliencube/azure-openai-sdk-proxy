﻿using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace AzureOpenAIProxy.ApiApp.Tests.Repositories;

public class AdminEventRepositoryTests
{


    [Fact]
    public void Given_ServiceCollection_When_AddAdminEventRepository_Invoked_Then_It_Should_Contain_AdminEventRepository()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAdminEventRepository();

        // Assert
        services.SingleOrDefault(p => p.ServiceType == typeof(IAdminEventRepository)).Should().NotBeNull();
    }

    [Fact]
    public void Given_Instance_When_CreateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventDetails = new AdminEventDetails();
        var repository = new AdminEventRepository();

        // Act
        Func<Task> func = async () => await repository.CreateEvent(eventDetails);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_GetEvents_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var repository = new AdminEventRepository();

        // Act
        Func<Task> func = async () => await repository.GetEvents();

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_GetEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var repository = new AdminEventRepository();

        // Act
        Func<Task> func = async () => await repository.GetEvent(eventId);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_UpdateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDetails = new AdminEventDetails();
        var repository = new AdminEventRepository();

        // Act
        Func<Task> func = async () => await repository.UpdateEvent(eventId, eventDetails);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    // TODO: [tae0y] Add tests for other methods
    [Fact]
    public async Task Given_TableServiceClientIsNull_When_CreateEvent_Invoked_Then_ItShould_Throw_NullReferenceException()
    {
        // Arrange
        var eventDetails = new AdminEventDetails();
        var repository = new AdminEventRepository();

        // Act
        Func<Task> func = async () => await repository.CreateEvent(eventDetails);

        // Assert
        await func.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task Given_AddEntityAsyncIsSuccessful_When_CreateEvent_Invoked_Then_ItShould_WorkProperly()
    {
        // Arrange
        var eventDetails = createRandomEventDetails(
                new AdminEventDetails(), 
                new HashSet<string> { }
            );
        // TODO: [tae0y] TableServiceClient를 Mocking하지 않고 실제 의존성을 주입
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient);

        // Act
        var result = await repository.CreateEvent(eventDetails);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Given_AddEntityAsyncIsNotSuccessful_When_CreateEvent_Invoked_Then_ItShould_ThrowException()
    {
        // Arrange
        // duplicate eventDetails
        var eventDetails = createRandomEventDetails(
                new AdminEventDetails(), 
                new HashSet<string> { }
            );
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient);
        var result = await repository.CreateEvent(eventDetails);

        // Act
        Func<Task> func = async () => await repository.CreateEvent(eventDetails);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Given_AddEntityAsyncIsSuccessful_When_CreateEvent_Invoked_Then_GetEntityAsyncShould_ReturnCorrectValue()
    {
        // Arrange
        var eventDetails = createRandomEventDetails(
                new AdminEventDetails(), 
                new HashSet<string> { }
            );
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient);

        // Act
        var result = await repository.CreateEvent(eventDetails);

        // Assert
        // result Should  BeEquivalentTo eventDetails except for PartitionKey, RowKey, Timestamp, ETag
        result.Should().BeEquivalentTo(eventDetails, options => options
            .Excluding(e => e.Timestamp)
            .Excluding(e => e.ETag)
        );
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

        if (!ShouldSkip(nameof(details.PartitionKey)) && string.IsNullOrEmpty(details.PartitionKey))
        {
            details.PartitionKey = details.TimeZone;
        }

        if (!ShouldSkip(nameof(details.RowKey)) && string.IsNullOrEmpty(details.RowKey))
        {
            details.RowKey = details.EventId.ToString();
        }

        return details;
    }
}
