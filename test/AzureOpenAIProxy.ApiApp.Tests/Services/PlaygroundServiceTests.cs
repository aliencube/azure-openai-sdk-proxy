using Azure;

using AzureOpenAIProxy.ApiApp.Repositories;
using AzureOpenAIProxy.ApiApp.Services;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AzureOpenAIProxy.ApiApp.Tests.Services;

public class PlaygroundServiceTests
{
    [Fact]
    public void Given_ServiceCollection_When_AddPlaygroundService_Invoked_Then_It_Should_Contain_PlaygroundService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddPlaygroundService();

        // Assert
        services.SingleOrDefault(p => p.ServiceType == typeof(IPlaygroundService)).Should().NotBeNull();
    }

    [Fact]
    public void Given_Instance_When_GetDeploymentModels_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var repository = Substitute.For<IEventRepository>();
        var service = new PlaygroundService(repository);

        // Act
        Func<Task> func = async () => await service.GetDeploymentModels(eventId);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task Given_Failure_In_Get_Entities_When_GetEvents_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var repository = Substitute.For<IEventRepository>();
        var service = new PlaygroundService(repository);

        repository.GetEvents().ThrowsAsync(new Exception("Error occurred"));

        // Act
        Func<Task> func = service.GetEvents;

        // Assert
        var assertion = await func.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Given_Exist_Events_When_GetEvents_Invoked_Then_It_Should_Return_EventDetails_List()
    {
        // Arrange
        Random rand = new();
        int listSize = rand.Next(1, 20);
        Guid eventId = new();
        var repository = Substitute.For<IEventRepository>();
        var service = new PlaygroundService(repository);

        var eventDetails = new EventDetails
        {
            RowKey = eventId.ToString()
        };

        List<EventDetails> events = [];
        for(int i = 0; i < listSize; ++i)
        {
            events.Add(eventDetails);
        }

        repository.GetEvents().Returns(events);

        // Act
        var result = await service.GetEvents();

        // Assert
        result.Count.Should().Be(listSize);
        result.First().Should().BeEquivalentTo(eventDetails);
    }

}