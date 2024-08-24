namespace AzureOpenAIProxy.ApiApp.Endpoints;

public static class AdminEndpointUrls
{
    /// <summary>
    /// Declares the admin events endpoint.
    /// </summary>
    public const string AdminEvents = "/admin/events";
    
    /// <summary>
    /// Declares the admin event details endpoint.
    /// </summary>
    public const string AdminEventDetails = "/admin/events/{eventId}";
}