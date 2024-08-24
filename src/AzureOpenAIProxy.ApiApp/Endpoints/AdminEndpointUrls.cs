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
    //TODO: [tae0y] endpoint 이름 정하기 /admin/events or /admin/eventlist
    public const string AdminEventList = "/admin/eventlist";
}