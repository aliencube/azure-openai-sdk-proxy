namespace AzureOpenAIProxy.ApiApp.Endpoints;

public static class AdminEndpointUrls
{
    /// <summary>
    /// Declares the admin event details endpoint.
    /// </summary>
    public const string AdminEventDetails = "/admin/events/{eventId}";

    /// <summary>
    /// Declares the admin event list endpoint.
    /// </summary>
    /// <remarks>
    /// - Get method for listing all events
    /// - Post method for new event creation
    /// </remarks>
    public const string AdminEvents = "/admin/events";
}