namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the collection of the playground endpoint URLs.
/// </summary>
public static class PlaygroundEndpointUrls
{
    /// <summary>
    /// Declares the event endpoint.
    /// </summary>
    /// <remarks>
    /// - GET method for listing all events
    /// </remarks>
    public const string Events = "/events";

    /// <summary>
    /// Declares the deployment models list endpoint.
    /// </summary>
    /// <remarks>
    /// - GET method for listing all deployment models
    /// </remarks>
    public const string DeploymentModels = "/events/{eventId}/deployment-models";
}
