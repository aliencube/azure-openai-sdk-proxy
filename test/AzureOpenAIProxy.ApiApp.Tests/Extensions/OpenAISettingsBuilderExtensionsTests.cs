using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Extensions;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace AzureOpenAIProxy.ApiApp.Tests.Extensions;

public class OpenAISettingsBuilderExtensionsTests
{
    [Fact]
    public void Given_Null_AppSettings_When_Invoked_WithAppSettings_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var config = default(IConfiguration);

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        Action action = () => builder.WithAppSettings(sp)
                                     .Build();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Empty_AzureSettings_When_Invoked_WithAppSettings_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure", string.Empty }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        Action action = () => builder.WithAppSettings(sp)
                                     .Build();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Empty_OpenAISettings_When_Invoked_WithAppSettings_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:OpenAI", string.Empty }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        Action action = () => builder.WithAppSettings(sp)
                                     .Build();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Null_OpenAIInstances_When_Invoked_WithAppSettings_Then_It_Should_Return_Empty_Instance()
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:OpenAI:Instances", string.Empty }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        var result = builder.WithAppSettings(sp)
                            .Build();

        // Assert
        result.Instances.Should().NotBeNull()
                             .And.BeEmpty();
    }

    [Fact]
    public void Given_AppSettings_When_Invoked_WithAppSettings_Then_It_Should_Return_Instance()
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:OpenAI:Instances:0", string.Empty },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        var result = builder.WithAppSettings(sp)
                            .Build();

        // Assert
        result.Instances.Should().NotBeNull()
                             .And.HaveCount(1);
    }

    [Theory]
    [InlineData("http://localhost")]
    public void Given_AppSettings_When_Invoked_WithAppSettings_Then_It_Should_Return_Endpoint(string endpoint)
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:OpenAI:Instances:0:Endpoint", endpoint },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        var result = builder.WithAppSettings(sp)
                            .Build();

        // Assert
        result.Instances.First().Endpoint.Should().Be(endpoint);
    }

    [Theory]
    [InlineData("api-key")]
    public void Given_AppSettings_When_Invoked_WithAppSettings_Then_It_Should_Return_ApiKey(string apiKey)
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:OpenAI:Instances:0:ApiKey", apiKey },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        var result = builder.WithAppSettings(sp)
                            .Build();

        // Assert
        result.Instances.First().ApiKey.Should().Be(apiKey);
    }

    [Theory]
    [InlineData("deployment-name-1")]
    [InlineData("deployment-name-1", "deployment-name-2")]
    [InlineData("deployment-name-1", "deployment-name-2", "deployment-name-3")]
    public void Given_AppSettings_When_Invoked_WithAppSettings_Then_It_Should_Return_Instances(params string[] deploymentNames)
    {
        // Arrange
        var dict = new Dictionary<string, string>();
        for (var i = 0; i < deploymentNames.Length; i++)
        {
            dict.Add($"Azure:OpenAI:Instances:0:DeploymentNames:{i}", deploymentNames[i]);
        }
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        var result = builder.WithAppSettings(sp)
                            .Build();

        // Assert
        result.Instances.First().DeploymentNames.Should().HaveCount(deploymentNames.Length);
        foreach (var dn in deploymentNames)
        {
            result.Instances.First().DeploymentNames.Should().Contain(dn);
        }
    }

    [Fact]
    public void Given_Empty_KeyVaultSettings_When_Invoked_WithKeyVault_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault", string.Empty }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        Action action = () => builder.WithKeyVault(sp)
                                     .Build();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData("http://localhost")]
    public void Given_Null_SecretClient_When_Invoked_WithKeyVault_Then_It_Should_Throw_Exception(string vaultUri)
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:VaultUri", vaultUri }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var client = new SecretClient(new Uri(vaultUri), new DefaultAzureCredential());

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        var builder = new OpenAISettingsBuilder();

        // Act
        Action action = () => builder.WithKeyVault(sp).Build();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }
}
