using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Configurations;

using FluentAssertions;

namespace AzureOpenAIProxy.ApiApp.Tests.Builders;

public class OpenAISettingsBuilderTests
{
    [Fact]
    public void Given_OpenAISettingsBuilder_When_Invoked_Build_Then_It_Should_Return_Instance()
    {
        // Arrange
        var builder = new OpenAISettingsBuilder();

        // Act
        var result = builder.Build();

        // Assert
        result.Should().NotBeNull().And.BeOfType<OpenAISettings>();
    }
}