using AzureOpenAIProxy.ApiApp.Extensions;

using Castle.Components.DictionaryAdapter;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace AzureOpenAIProxy.ApiApp.Tests.Extensions;

public class OpenApiSettingsExtensionsTests
{
    [Fact]
    public void Given_Null_OpenApiSettings_When_Added_ToServiceProvider_Then_It_Should_Throw_Exception()
    {
        // Arrange
        var config = default(IConfiguration);

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        // Act
        Action action = () => sp.GetOpenApiSettings();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("v1.0.0")]
    public void Given_OpenApiSettings_When_Added_ToServiceProvider_Then_It_Should_Return_DocVersion(string docVersion)
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "OpenApi:DocVersion", docVersion }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sp = Substitute.For<IServiceProvider>();
        ServiceProviderServiceExtensions.GetService<IConfiguration>(sp).Returns(config);

        // Act
        var result = sp.GetOpenApiSettings();

        // Assert
        result.DocVersion.Should().Be(docVersion);
    }

    [Theory]
    [InlineData("")]
    [InlineData("v1.0.0")]
    public void Given_OpenApiSettings_When_Added_ToServiceCollection_Then_It_Should_Return_DocVersion(string docVersion)
    {
        // Arrange
        var dict = new Dictionary<string, string>()
        {
            { "OpenApi:DocVersion", docVersion }
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        var config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        var sc = new ServiceCollection();
        sc.AddSingleton<IConfiguration>(config);

        // Act
        var result = sc.GetOpenApiSettings();

        // Assert
        result.DocVersion.Should().Be(docVersion);
    }
}