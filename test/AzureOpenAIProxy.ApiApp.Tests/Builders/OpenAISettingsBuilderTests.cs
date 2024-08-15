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
        result.Should().NotBeNull()
                   .And.BeOfType<OpenAISettings>();
    }

    [Fact]
    public void Given_OpenAISettingsBuilder_When_Invoked_Build_Then_It_Should_Return_Empty()
    {
        // Arrange
        var builder = new OpenAISettingsBuilder();

        // Act
        var result = builder.Build();

        // Assert
        result.Instances.Should().NotBeNull()
                             .And.BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void Given_OpenAISettingsBuilder_When_Invoked_SetOpenAIInstances_Then_It_Should_Set_Instances(int count)
    {
        // Arrange
        var builder = new OpenAISettingsBuilder();
        var instances = Enumerable.Range(0, count).Select(p => new OpenAIInstanceSettings()).ToList();

        // Act
        builder.SetOpenAIInstances(instances);
        var result = builder.Build();

        // Assert
        result.Instances.Should().HaveCount(count);
    }

    [Fact]
    public void Given_OpenAISettingsBuilder_When_Invoked_SetOpenAIInstances_Then_It_Should_Return_Empty()
    {
        // Arrange
        var builder = new OpenAISettingsBuilder();

        // Act
        builder.SetOpenAIInstances(null);
        var result = builder.Build();

        // Assert
        result.Instances.Should().NotBeNull()
                             .And.BeEmpty();
    }
}