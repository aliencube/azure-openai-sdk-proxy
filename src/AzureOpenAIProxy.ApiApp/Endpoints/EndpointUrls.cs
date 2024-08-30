namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the collection of the endpoint URLs.
/// </summary>
public static class EndpointUrls
{
    /// <summary>
    /// Declares the weather forecast endpoint.
    /// </summary>
    public const string WeatherForecast = "/weatherforecast";

    /// <summary>
    /// Declares the chat completions endpoint.
    /// </summary>
    public const string ChatCompletions = "/openai/deployments/{deploymentName}/chat/completions";

    /// <summary>
    /// Declares the event endpoint.
    /// </summary>
    public const string Events = "/events";
}
