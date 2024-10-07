using Azure;

using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;
using AzureOpenAIProxy.ApiApp.Services;

using FluentAssertions;

using Microsoft.AspNetCore.Authentication;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

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
    public async Task Given_Instance_When_CreateEvent_Invoked_Then_It_Should_Return_Same_Instance()
    {
        // Arrange
        var eventDetails = new AdminEventDetails();
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);

        repository.CreateEvent(Arg.Any<AdminEventDetails>()).Returns(eventDetails);

        // Act
        var result = await service.CreateEvent(eventDetails);

        // Assert
        result.Should().BeEquivalentTo(
            eventDetails,
            options => options.Excluding(x => x.PartitionKey)
                              .Excluding(x => x.RowKey)
        );
    }

    [Fact]
    public async Task Given_Failure_In_Add_Entity_When_CreateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var eventDetails = new AdminEventDetails();
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);

        var exception = new InvalidOperationException("Invalid Operation Error : check duplicate, null or empty values");
        repository.CreateEvent(Arg.Any<AdminEventDetails>()).ThrowsAsync(exception);

        // Act
        Func<Task> func = () => service.CreateEvent(eventDetails);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
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


    [Theory]
    [InlineData(404)]
    [InlineData(500)]
    public async Task Given_Failure_In_Get_Entity_When_GetEvent_Invoked_Then_It_Should_Throw_Exception(int statusCode)
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);

        var exception = new RequestFailedException(statusCode, "Request Failed", default, default);

        repository.GetEvent(Arg.Any<Guid>()).ThrowsAsync(exception);

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
        var repository = Substitute.For<IAdminEventRepository>();
        var service = new AdminEventService(repository);

        var eventDetails = new AdminEventDetails
        {
            RowKey = eventId
        };

        var guid = Guid.Parse(eventId);

        repository.GetEvent(guid).Returns(Task.FromResult(eventDetails));

        // Act
        var result = await service.GetEvent(guid);

        // Assert
        result.Should().BeEquivalentTo(eventDetails);
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
}
