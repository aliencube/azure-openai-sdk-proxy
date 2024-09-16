using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;

using Castle.Core.Configuration;

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

    [Fact]
    public void Given_Instance_When_GetEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var eventId = Guid.NewGuid();
        var repository = new AdminEventRepository(tableServiceClient, settings);

        // Act
        Func<Task> func = async () => await repository.GetEvent(eventId);

        // Assert
        func.Should().ThrowAsync<NotImplementedException>();
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
}
