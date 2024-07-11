using System.Reflection;

using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Configurations;

using FluentAssertions;

namespace AzureOpenAIProxy.ApiApp.Tests.Builders;

public class OpenAIServiceRequestBuilderTests
{
    [Theory]
    [InlineData("https://localhost", "my-api-key", "deployment-name-1")]
    public void Given_OpenAISettings_When_Invoked_SetOpenAIInstance_Then_It_Should_Store_Value(string endpoint, string apiKey, string deploymentName)
    {
        // Arrange
        var instance = new OpenAIInstanceSettings
        {
            Endpoint = endpoint,
            ApiKey = apiKey,
            DeploymentNames = new List<string>() { deploymentName },
        };
        var settings = new OpenAISettings()
        {
            Instances = { instance }
        };

        // Act
        var builder = new OpenAIServiceRequestBuilder();
        builder.SetOpenAIInstance(settings, deploymentName);

        // Assert
        var _endpoint = builder.GetType().GetField("_endpoint", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(builder) as string;
        var _apiKey = builder.GetType().GetField("_apiKey", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(builder) as string;

        _endpoint.Should().Be(endpoint);
        _apiKey.Should().Be(apiKey);
    }

    [Theory]
    [InlineData("https://localhost", "my-api-key", "deployment-name-1", "deployment-name-2")]
    public void Given_OpenAISettings_When_Invoked_SetOpenAIInstance_Then_It_Should_Return_Null(string endpoint, string apiKey, string deploymentName, string nonDeploymentName)
    {
        // Arrange
        var instance = new OpenAIInstanceSettings
        {
            Endpoint = endpoint,
            ApiKey = apiKey,
            DeploymentNames = new List<string>() { deploymentName },
        };
        var settings = new OpenAISettings()
        {
            Instances = { instance }
        };

        // Act
        var builder = new OpenAIServiceRequestBuilder();
        builder.SetOpenAIInstance(settings, nonDeploymentName);

        // Assert
        var _endpoint = builder.GetType().GetField("_endpoint", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(builder) as string;
        var _apiKey = builder.GetType().GetField("_apiKey", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(builder) as string;

        _endpoint.Should().BeNull();
        _apiKey.Should().BeNull();
    }
}