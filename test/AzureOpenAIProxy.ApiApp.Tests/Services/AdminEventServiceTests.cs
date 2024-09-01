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
}
