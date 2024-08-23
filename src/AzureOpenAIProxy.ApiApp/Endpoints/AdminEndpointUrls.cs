namespace AzureOpenAIProxy.ApiApp.Endpoints;

public static class AdminEndpointUrls
{
    /// <summary>
    /// Declares the admin events endpoint.
    /// </summary>
    public const string AdminEvent = "/admin/events";
    public const string AdminEventDetails = "/admin/events/{eventId}";
}