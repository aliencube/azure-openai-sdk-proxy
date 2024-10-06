using System.Linq.Expressions;

using Azure;
using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Repositories;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AzureOpenAIProxy.ApiApp.Tests.Repositories;

public class EventRepositoryTests
{
    [Fact]
    public void Given_ServiceCollection_When_AddEventRepository_Invoked_Then_It_Should_Contain_EventRepository()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddEventRepository();

        // Assert
        services.SingleOrDefault(p => p.ServiceType == typeof(IEventRepository)).Should().NotBeNull();
    }

    [Fact]
    public void Given_Null_TableServiceClient_When_Creating_EventRepository_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = default(TableServiceClient);
        
        // Act
        Action action = () => new EventRepository(tableServiceClient, settings);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Given_Null_StorageAccountSettings_When_Creating_EventRepository_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = default(StorageAccountSettings);
        var tableServiceClient = Substitute.For<TableServiceClient>();
        
        // Act
        Action action = () => new EventRepository(tableServiceClient, settings);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(404)]
    [InlineData(500)]
    public async Task Given_Failure_In_Get_Entities_When_GetEvents_Invoked_Then_It_Should_Throw_Exception(int statusCode)
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new EventRepository(tableServiceClient, settings);

        var exception = new RequestFailedException(statusCode, "Request Error", default, default);

        var tableClient = Substitute.For<TableClient>();
        tableServiceClient.GetTableClient(Arg.Any<string>()).Returns(tableClient);
        tableClient.QueryAsync(Arg.Any<Expression<Func<EventDetails, bool>>>()).Throws(exception);

        // Act
        Func<Task> func = repository.GetEvents;

        // Assert
        var assertion = await func.Should().ThrowAsync<RequestFailedException>();
        assertion.Which.Status.Should().Be(statusCode);
    }

    [Fact]
    public async Task Given_Exist_Events_When_GetEvents_Invoked_Then_It_Should_Return_EventDetails_List()
    {
        // Arrange
        Random rand = new();
        int listSize = rand.Next(0, 20);
        Guid eventId = new();

        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new EventRepository(tableServiceClient, settings);

        var eventDetails = new EventDetails
        {
            RowKey = eventId.ToString(),
            PartitionKey = PartitionKeys.EventDetails
        };

        List<EventDetails> events = [];

        for(int i = 0; i < listSize; ++i)
        {
            events.Add(eventDetails);
        }

        var pages = Page<EventDetails>.FromValues(events, default, Substitute.For<Response>());
        var asyncPages = AsyncPageable<EventDetails>.FromPages([pages]);

        var tableClient = Substitute.For<TableClient>();
        tableServiceClient.GetTableClient(Arg.Any<string>()).Returns(tableClient);
        tableClient.QueryAsync(Arg.Any<Expression<Func<EventDetails, bool>>>()).Returns(asyncPages);

        // Act
        var result = await repository.GetEvents();

        // Assert
        result.Count.Should().Be(listSize);
        result.First().Should().BeEquivalentTo(eventDetails);
    }

}