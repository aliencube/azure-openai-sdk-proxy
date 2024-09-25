using Azure;
using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

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
    public void Given_Null_TableServiceClient_When_Creating_AdminEventRepository_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = default(TableServiceClient);
        
        // Act
        Action action = () => new AdminEventRepository(tableServiceClient, settings);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Given_Null_StorageAccountSettings_When_Creating_AdminEventRepository_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = default(StorageAccountSettings);
        var tableServiceClient = Substitute.For<TableServiceClient>();
        
        // Act
        Action action = () => new AdminEventRepository(tableServiceClient, settings);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Given_Instance_When_CreateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var eventDetails = new AdminEventDetails();
        var repository = new AdminEventRepository(tableServiceClient, settings);

        // Act
        Func<Task> func = async () => await repository.CreateEvent(eventDetails);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_GetEvents_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient, settings);

        // Act
        Func<Task> func = async () => await repository.GetEvents();

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Theory]
    [InlineData(404)]
    [InlineData(500)]
    public async Task Given_Failure_In_Get_Entity_When_GetEvent_Invoked_Then_It_Should_Throw_Exception(int statusCode)
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var eventId = Guid.NewGuid();
        var repository = new AdminEventRepository(tableServiceClient, settings);

        var exception = new RequestFailedException(statusCode, "Request Error", default, default);

        var tableClient = Substitute.For<TableClient>();
        tableServiceClient.GetTableClient(Arg.Any<string>()).Returns(tableClient);
        tableClient.GetEntityAsync<AdminEventDetails>(Arg.Any<string>(), Arg.Any<string>())
            .ThrowsAsync(exception);

        // Act
        Func<Task> func = () => repository.GetEvent(eventId);

        // Assert
        var assertion = await func.Should().ThrowAsync<RequestFailedException>();
        assertion.Which.Status.Should().Be(statusCode);
    }

    [Theory]
    [InlineData("c355cc28-d847-4637-aad9-2f03d39aa51f", "event-details")]
    public async Task Given_Exist_EventId_When_GetEvent_Invoked_Then_It_Should_Return_AdminEventDetails(string eventId, string partitionKey)
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient, settings);

        var eventDetails = new AdminEventDetails
        {
            RowKey = eventId,
            PartitionKey = partitionKey
        };

        var response = Response.FromValue(eventDetails, Substitute.For<Response>());

        var tableClient = Substitute.For<TableClient>();
        tableServiceClient.GetTableClient(Arg.Any<string>()).Returns(tableClient);
        tableClient.GetEntityAsync<AdminEventDetails>(partitionKey, eventId)
            .Returns(Task.FromResult(response));

        // Act
        var result = await repository.GetEvent(Guid.Parse(eventId));

        // Assert
        result.Should().BeEquivalentTo(eventDetails);
    }

    [Fact]
    public void Given_Instance_When_UpdateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var eventId = Guid.NewGuid();
        var eventDetails = new AdminEventDetails();
        var repository = new AdminEventRepository(tableServiceClient, settings);

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
        var eventDetails = createRandomEventDetails(
                new AdminEventDetails(), 
                new HashSet<string> { }
            );
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient, settings);

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
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient, settings);

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
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient, settings);

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
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient, settings);

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
