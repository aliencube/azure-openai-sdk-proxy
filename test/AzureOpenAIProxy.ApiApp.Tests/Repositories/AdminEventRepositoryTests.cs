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
    public async Task Given_Instance_When_CreateEvent_Invoked_Then_It_Should_Return_Same_Instance()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var tableClient = Substitute.For<TableClient>();
        tableServiceClient.GetTableClient(Arg.Any<string>()).Returns(tableClient);
        var eventDetails = new AdminEventDetails();
        var repository = new AdminEventRepository(tableServiceClient, settings);

        // Act
        var result = await repository.CreateEvent(eventDetails);

        // Assert
        result.Should().BeSameAs(eventDetails);
    }

    [Fact]
    public async Task Given_Failure_In_Add_Entity_When_CreateEvent_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var repository = new AdminEventRepository(tableServiceClient, settings);
        var eventDetails = new AdminEventDetails();

        var exception = new InvalidOperationException("Invalid Operation Error : check duplicate, null or empty values");

        var tableClient = Substitute.For<TableClient>();
        tableServiceClient.GetTableClient(Arg.Any<string>()).Returns(tableClient);
        tableClient.AddEntityAsync<AdminEventDetails>(Arg.Any<AdminEventDetails>())
            .ThrowsAsync(exception);

        // Act
        Func<Task> func = () => repository.CreateEvent(eventDetails);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
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
}
