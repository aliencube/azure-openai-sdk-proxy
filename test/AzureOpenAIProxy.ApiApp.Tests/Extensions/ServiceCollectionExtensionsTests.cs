using Azure;
using Azure.Data.Tables;
using Azure.Security.KeyVault.Secrets;

using AzureOpenAIProxy.ApiApp.Extensions;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace AzureOpenAIProxy.ApiApp.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void Given_ServiceCollection_When_Invoked_AddKeyVaultService_Then_It_Should_Contain_SecretClient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddKeyVaultService();

        // Assert
        services.SingleOrDefault(p => p.ServiceType == typeof(SecretClient)).Should().NotBeNull();
    }

    [Fact]
    public void Given_ServiceCollection_When_Invoked_AddKeyVaultService_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddKeyVaultService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<SecretClient>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Empty_AzureSettings_When_Invoked_AddKeyVaultService_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure", string.Empty },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddKeyVaultService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<SecretClient>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Empty_KeyVaultSettings_When_Invoked_AddKeyVaultService_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault", string.Empty },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddKeyVaultService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<SecretClient>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(default(string), "secret-name")]
    [InlineData("", "secret-name")]
    public void Given_NullOrEmpty_VaultUri_When_Invoked_AddKeyVaultService_Then_It_Should_Throw_Exception(string? vaultUri, string secretName)
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:VaultUri", vaultUri! },
            { "Azure:KeyVault:SecretNames:OpenAI", secretName },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddKeyVaultService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<SecretClient>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData("http://localhost", default(string), typeof(KeyNotFoundException))]
    [InlineData("http://localhost", "", typeof(InvalidOperationException))]
    public void Given_NullOrEmpty_SecretName_When_Invoked_AddKeyVaultService_Then_It_Should_Throw_Exception(string vaultUri, string? secretName, Type exceptionType)
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:VaultUri", vaultUri },
            { "Azure:KeyVault:SecretNames:OpenAI", secretName! },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddKeyVaultService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<SecretClient>();

        // Assert
        action.Should().Throw<Exception>().Which.Should().BeOfType(exceptionType);
    }

    [Theory]
    [InlineData("abcde", "secret-name")]
    public void Given_Invalid_VaultUri_When_Invoked_AddKeyVaultService_Then_It_Should_Throw_Exception(string vaultUri, string secretName)
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:VaultUri", vaultUri },
            { "Azure:KeyVault:SecretNames:OpenAI", secretName },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddKeyVaultService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<SecretClient>();

        // Assert
        action.Should().Throw<UriFormatException>();
    }

    [Theory]
    [InlineData("http://localhost", "secret-name")]
    public void Given_AppSettings_When_Invoked_AddKeyVaultService_Then_It_Should_Return_SecretClient(string vaultUri, string secretName)
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:VaultUri", vaultUri },
            { "Azure:KeyVault:SecretNames:OpenAI", secretName },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddKeyVaultService();

        // Act
        var result = services.BuildServiceProvider().GetService<SecretClient>();

        // Assert
        result.Should().NotBeNull()
                   .And.BeOfType<SecretClient>();
    }

    [Theory]
    [InlineData("http://localhost", "secret-name")]
    public void Given_AppSettings_When_Invoked_AddKeyVaultService_Then_It_Should_Return_VaultUri(string vaultUri, string secretName)
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:VaultUri", vaultUri },
            { "Azure:KeyVault:SecretNames:OpenAI", secretName },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddKeyVaultService();

        var expected = new Uri(vaultUri);

        // Act
        var result = services.BuildServiceProvider().GetService<SecretClient>();

        // Assert
        result?.VaultUri.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Given_ServiceCollection_When_Invoked_AddTableStorageService_Then_It_Should_Contain_TableServiceClient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddTableStorageService();

        // Assert
        services.SingleOrDefault(p => p.ServiceType == typeof(TableServiceClient)).Should().NotBeNull();
    }

    [Fact]
    public void Given_ServiceCollection_When_Invoked_AddTableStorageService_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTableStorageService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<TableServiceClient>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Empty_AzureSettings_When_Invoked_AddTableStorageService_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure", string.Empty},
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddTableStorageService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<TableServiceClient>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Empty_KeyVaultSettings_When_Invoked_AddTableStorageService_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault", string.Empty },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddTableStorageService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<TableServiceClient>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Missing_SecretClient_When_Invoked_AddTableStorageService_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:SecretNames:Storage", "secret-name" },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);
        services.AddTableStorageService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<TableServiceClient>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(default(string), typeof(KeyNotFoundException))]
    [InlineData("", typeof(InvalidOperationException))]
    public void Given_NullOrEmpty_SecretName_When_Invoked_AddTableStorageService_Then_It_Shoud_Throw_Exception(string? secretName, Type exceptionType)
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:SecretNames:Storage", secretName },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);

        var sc = Substitute.For<SecretClient>();
        services.AddSingleton(sc);

        services.AddTableStorageService();

        // Act
        Action action = () => services.BuildServiceProvider().GetService<TableServiceClient>();

        // Assert
        action.Should().Throw<Exception>().Which.Should().BeOfType(exceptionType);
    }

    [Theory]
    [InlineData("secret-name", "DefaultEndpointsProtocol=https;AccountName=account;AccountKey=ZmFrZWtleQ==;EndpointSuffix=core.windows.net")]
    public void Given_AppSettings_When_Invoked_AddTableStorageService_Then_It_Should_Return_TableServiceClient(string secretName, string connectionString)
    {
        // Arrange
        var services = new ServiceCollection();
        var dict = new Dictionary<string, string>()
        {
            { "Azure:KeyVault:SecretNames:Storage", secretName },
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        services.AddSingleton<IConfiguration>(config);

        var sc = Substitute.For<SecretClient>();
        var sp = new SecretProperties(secretName);
        var secret = SecretModelFactory.KeyVaultSecret(sp,connectionString);

        sc.GetSecret(secretName).Returns(Response.FromValue(secret, Substitute.For<Response>()));
        services.AddSingleton(sc);

        services.AddTableStorageService();

        // Act
        var result = services.BuildServiceProvider().GetService<TableServiceClient>();

        // Assert
        result.Should().NotBeNull()
                    .And.BeOfType<TableServiceClient>();
    }
}