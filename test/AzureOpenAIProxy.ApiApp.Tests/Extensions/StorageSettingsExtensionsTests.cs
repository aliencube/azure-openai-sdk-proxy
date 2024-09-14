using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Extensions;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureOpenAIProxy.ApiApp.Tests.Extensions;

public class StorageAccountSettingsExtensionsTests
{
    [Fact]
    public void Given_Empty_AzureSettings_When_Added_ToServiceCollection_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure", string.Empty }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sc = new ServiceCollection();
        sc.AddSingleton<IConfiguration>(config);

        sc.AddStorageAccountSettings();

        // Act
        Action action = () => sc.BuildServiceProvider().GetService<StorageAccountSettings>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Empty_StorageAccountSettings_When_Added_ToServiceCollection_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:StorageAccount", string.Empty }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sc = new ServiceCollection();
        sc.AddSingleton<IConfiguration>(config);

        sc.AddStorageAccountSettings();

        // Act
        Action action = () => sc.BuildServiceProvider().GetService<StorageAccountSettings>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Given_Empty_TableStorageSettings_When_Added_ToServiceCollection_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:StorageAccount:TableStorage", string.Empty }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sc = new ServiceCollection();
        sc.AddSingleton<IConfiguration>(config);

        sc.AddStorageAccountSettings();

        // Act
        Action action = () => sc.BuildServiceProvider().GetService<StorageAccountSettings>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(default(string))]
    [InlineData("")]
    public void Given_NullOrEmpty_TableName_When_Added_ToServiceColleciton_Then_It_Should_Throw_Exception(string? tableName)
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:StorageAccount:TableStorage:TableName", tableName }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sc = new ServiceCollection();
        sc.AddSingleton<IConfiguration>(config);

        sc.AddStorageAccountSettings();

        // Act
        Action action = () => sc.BuildServiceProvider().GetService<StorageAccountSettings>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData("table-name")]
    public void Given_Appsettings_When_Added_ToServiceCollection_Then_It_Should_Return_StorageAccountSettings(string tableName)
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "Azure:StorageAccount:TableStorage:TableName", tableName }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sc = new ServiceCollection();
        sc.AddSingleton<IConfiguration>(config);

        sc.AddStorageAccountSettings();

        // Act
        var settings = sc.BuildServiceProvider().GetService<StorageAccountSettings>();

        // Assert
        settings?.TableStorage.TableName.Should().BeEquivalentTo(tableName);
    }
}