using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;
using AzureOpenAIProxy.ApiApp.Services;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AzureOpenAIProxy.ApiApp.Tests.Services;

public class AdminResourceServiceTests
{
    [Fact]
    public void Given_ServiceCollection_When_AddAdminResourceService_Invoked_Then_It_Should_Contain_AdminResourceService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAdminResourceService();

        // Assert
        services.SingleOrDefault(p => p.ServiceType == typeof(IAdminResourceService)).Should().NotBeNull();
    }

    [Fact]
    public void Given_Null_Repository_When_Creating_AdminResourceService_Then_It_Should_Throw_Exception()
    {
        // Arrange
        IAdminResourceRepository? repository = null;

        // Act
        Action action = () => new AdminResourceService(repository!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task Given_Instance_When_CreateResource_Invoked_Then_It_Should_Add_Entity()
    {
        // Arrange
        var repository = Substitute.For<IAdminResourceRepository>();
        var service = new AdminResourceService(repository);

        var resourceDetails = new AdminResourceDetails
        {
            ResourceId = Guid.NewGuid(),
            FriendlyName = "Test Resource",
            DeploymentName = "Test Deployment",
            ResourceType = ResourceType.Chat,
            Endpoint = "https://test.endpoint.com",
            ApiKey = "test-api-key",
            Region = "test-region",
            IsActive = true
        };

        repository.CreateResource(resourceDetails).Returns(resourceDetails);

        // Act
        var result = await service.CreateResource(resourceDetails);

        // Assert
        await repository.Received(1).CreateResource(Arg.Is<AdminResourceDetails>(x =>
            x.ResourceId == resourceDetails.ResourceId
        ));

        result.Should().BeEquivalentTo(resourceDetails);
    }

    [Fact]
    public async Task Given_Failure_In_Add_Entity_When_CreateResource_Invoked_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var repository = Substitute.For<IAdminResourceRepository>();
        var service = new AdminResourceService(repository);

        var resourceDetails = new AdminResourceDetails
        {
            ResourceId = Guid.NewGuid(),
            FriendlyName = "Test Resource",
            DeploymentName = "Test Deployment",
            ResourceType = ResourceType.Chat,
            Endpoint = "https://test.endpoint.com",
            ApiKey = "test-api-key",
            Region = "test-region",
            IsActive = true
        };

        repository.CreateResource(Arg.Any<AdminResourceDetails>()).ThrowsAsync(new InvalidOperationException());

        // Act
        Func<Task> act = async () => await service.CreateResource(resourceDetails);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}