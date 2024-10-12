using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace AzureOpenAIProxy.ApiApp.Tests.Repositories;

public class AdminResourceRepositoryTests
{
    [Fact]
    public void Given_ServiceCollection_When_AddAdminResourceRepository_Invoked_Then_It_Should_Contain_AdminResourceRepository()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAdminResourceRepository();

        // Assert
        services.SingleOrDefault(p => p.ServiceType == typeof(IAdminResourceRepository)).Should().NotBeNull();
    }

    [Fact]
    public void Given_Null_TableServiceClient_When_Creating_AdminResourceRepository_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = default(TableServiceClient);
        
        // Act
        Action action = () => new AdminResourceRepository(tableServiceClient!, settings);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Given_Null_StorageAccountSettings_When_Creating_AdminResourceRepository_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var settings = default(StorageAccountSettings);
        var tableServiceClient = Substitute.For<TableServiceClient>();
        
        // Act
        Action action = () => new AdminResourceRepository(tableServiceClient, settings!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task Given_Instance_When_CreateResource_Invoked_Then_It_Should_Add_Entity()
    {
        // Arrange
        var settings = Substitute.For<StorageAccountSettings>();
        var tableServiceClient = Substitute.For<TableServiceClient>();
        var tableClient = Substitute.For<TableClient>();
        tableServiceClient.GetTableClient(Arg.Any<string>()).Returns(tableClient);

        var repository = new AdminResourceRepository(tableServiceClient, settings);

        var resourceId = Guid.NewGuid();
        var resourceDetails = new AdminResourceDetails
        {
            ResourceId = resourceId,
            FriendlyName = "Test Resource",
            DeploymentName = "Test Deployment",
            ResourceType = ResourceType.Chat,
            Endpoint = "https://test.endpoint.com",
            ApiKey = "test-api-key",
            Region = "test-region",
            IsActive = true,
            PartitionKey = PartitionKeys.ResourceDetails,
            RowKey = resourceId.ToString()
        };

        // Act
        var result = await repository.CreateResource(resourceDetails);

        // Assert
        await tableClient.Received(1).AddEntityAsync(Arg.Is<AdminResourceDetails>(x =>
            x.ResourceId == resourceDetails.ResourceId
        ));
        result.Should().BeEquivalentTo(resourceDetails);
    }
}