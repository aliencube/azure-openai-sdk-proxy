using Azure;

using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;
using AzureOpenAIProxy.ApiApp.Services;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AzureOpenAIProxy.ApiApp.Tests.Services;

public class AdminEventServiceTests
{
    private readonly IAdminEventRepository mockRepository;

    public AdminEventServiceTests()
    {
        mockRepository = Substitute.For<IAdminEventRepository>();
    }

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
        var service = new AdminEventService(mockRepository);

        // Act
        Func<Task> func = async () => await service.CreateEvent(eventDetails);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Given_Instance_When_GetEvents_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var service = new AdminEventService(mockRepository);

        // Act
        Func<Task> func = async () => await service.GetEvents();

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
    }


    [Theory]
    [InlineData(404)]
    [InlineData(500)]
    public async Task Given_Failure_In_Get_Entity_When_GetEvent_Invoked_Then_It_Should_Throw_Exception(int statusCode)
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var service = new AdminEventService(mockRepository);

        var exception = new RequestFailedException(statusCode, "Request Failed", default, default);

        mockRepository.GetEvent(Arg.Any<Guid>()).ThrowsAsync(exception);

        // Act
        Func<Task> func = () => service.GetEvent(eventId);

        // Assert
        var assertion = await func.Should().ThrowAsync<RequestFailedException>();
        assertion.Which.Status.Should().Be(statusCode);
    }

    [Theory]
    [InlineData("c355cc28-d847-4637-aad9-2f03d39aa51f")]
    public async Task Given_Exist_EventId_When_GetEvent_Invoked_Then_It_Should_Return_AdminEventDetails(string eventId)
    {
        // Arrange
        var service = new AdminEventService(mockRepository);

        var eventDetails = new AdminEventDetails
        {
            RowKey = eventId
        };

        var guid = Guid.Parse(eventId);

        mockRepository.GetEvent(guid).Returns(Task.FromResult(eventDetails));

        // Act
        var result = await service.GetEvent(guid);

        // Assert
        result.Should().BeEquivalentTo(eventDetails);
    }

    [Fact]
    public async Task Given_Instance_When_UpdateEvent_Invoked_Then_It_Should_Return_Updated_Event_Details()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDetails = new AdminEventDetails();
        var service = new AdminEventService(mockRepository);

        mockRepository.UpdateEvent(eventId, eventDetails).Returns(eventDetails);

        // Act
        var updatedEventDetails = await service.UpdateEvent(eventId, eventDetails);

        // Assert
        updatedEventDetails.Should().BeEquivalentTo(eventDetails);
    }

    [Fact]
    public async Task Given_Instance_When_DeleteEvent_Invoked_Then_It_Should_Return_Deleted_Event_Id()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDetails = new AdminEventDetails();
        var service = new AdminEventService(mockRepository);

        eventDetails.EventId = eventId;
        mockRepository.DeleteEvent(eventDetails).Returns(eventDetails.EventId);

        // Act
        var deletedEventId = await service.DeleteEvent(eventDetails);

        // Assert
        deletedEventId.Should().Be(eventId);
    }
}
