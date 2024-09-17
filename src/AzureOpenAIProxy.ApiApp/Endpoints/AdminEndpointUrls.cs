namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the collection of the admin endpoint URLs.
/// </summary>
public static class AdminEndpointUrls
{
    /// <summary>
    /// Declares the admin event details endpoint.
    /// </summary>
    /// <remarks>
    /// - GET method for an event details
    /// - PUT method for update an event details
    /// </remarks>
    public const string AdminEventDetails = "/admin/events/{eventId}";

    /// <summary>
    /// Declares the admin event list endpoint.
    /// </summary>
    /// <remarks>
    /// - GET method for listing all events
    /// - POST method for new event creation
    /// </remarks>
    public const string AdminEvents = "/admin/events";

    /// <summary>
    /// Declares the admin resource details endpoint.
    /// </summary>
    /// <remarks>
    /// - POST method for new resource creation
    /// </remarks>
    public const string AdminResources = "/admin/resources";
}