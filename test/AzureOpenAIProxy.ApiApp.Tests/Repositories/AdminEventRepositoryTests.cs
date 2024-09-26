using System.Configuration;

using Azure;
using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Xunit.Sdk;

namespace AzureOpenAIProxy.ApiApp.Tests.Repositories;

public class AdminEventRepositoryTests
{
    private StorageAccountSettings mockSettings;
    private TableServiceClient mockTableServiceClient;
    private TableClient mockTableClient;

    public AdminEventRepositoryTests()
    {
        mockSettings = Substitute.For<StorageAccountSettings>();
        mockTableServiceClient = Substitute.For<TableServiceClient>();
        mockTableClient = Substitute.For<TableClient>();

        mockTableServiceClient.GetTableClient(Arg.Any<string>()).Returns(mockTableClient);
    }

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
        var tableServiceClient = default(TableServiceClient);
        
        // Act
        Action action = () => new AdminEventRepository(tableServiceClient, mockSettings);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Given_Null_StorageAccountSettings_When_Creating_AdminEventRepository_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = default(StorageAccountSettings);
        
        // Act
        Action action = () => new AdminEventRepository(mockTableServiceClient, settings);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task Given_Instance_When_CreateEvent_Invoked_Then_It_Should_Return_Created_Event()
    {
        // Arrange
        var eventDetails = new AdminEventDetails();
        var repository = new AdminEventRepository(mockTableServiceClient, mockSettings);

        // Act
        await repository.CreateEvent(eventDetails);

        // Assert
        await mockTableClient.Received(1).AddEntityAsync(eventDetails, default);
    }

    [Fact]
    public async Task Given_Instance_When_GetEvents_Invoked_Then_It_Should_Invoke_QueryAsync_Method()
    {
        // Arrange
        var repository = new AdminEventRepository(mockTableServiceClient, mockSettings);

        // Act
        await repository.GetEvents();

        // Assert
        mockTableClient.Received(1).QueryAsync<AdminEventDetails>();
    }

    [Theory]
    [InlineData(404)]
    [InlineData(500)]
    public async Task Given_Failure_In_Get_Entity_When_GetEvent_Invoked_Then_It_Should_Throw_Exception(int statusCode)
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var repository = new AdminEventRepository(mockTableServiceClient, mockSettings);

        var exception = new RequestFailedException(statusCode, "Request Error", default, default);

        mockTableClient.GetEntityAsync<AdminEventDetails>(Arg.Any<string>(), Arg.Any<string>())
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
        var repository = new AdminEventRepository(mockTableServiceClient, mockSettings);

        var eventDetails = new AdminEventDetails
        {
            RowKey = eventId,
            PartitionKey = partitionKey
        };

        var response = Response.FromValue(eventDetails, Substitute.For<Response>());

        mockTableClient.GetEntityAsync<AdminEventDetails>(partitionKey, eventId)
            .Returns(Task.FromResult(response));

        // Act
        var result = await repository.GetEvent(Guid.Parse(eventId));

        // Assert
        result.Should().BeEquivalentTo(eventDetails);
    }

    [Fact]
    public async Task Given_Instance_When_UpdateEvent_Invoked_Then_It_Should_Invoke_UpdateEntityAsync_Method()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDetails = new AdminEventDetails();
        var repository = new AdminEventRepository(mockTableServiceClient, mockSettings);

        // Act
        await repository.UpdateEvent(eventId, eventDetails);

        // Assert
        await mockTableClient.Received(1)
                    .UpdateEntityAsync<AdminEventDetails>(Arg.Any<AdminEventDetails>(),
                                                          Arg.Any<Azure.ETag>(),
                                                          TableUpdateMode.Replace);
    }
}
