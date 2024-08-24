namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represent the event detail data for response by admin event endpoint.
/// </summary>
public class AdminEventDetails : EventDetails
{
    /// <summary>
    /// Gets or sets the event coorganizer name.
    /// </summary>
    public string? CoorganizerName { get; set; }

    /// <summary>
    /// Gets or sets the event coorganizer email.
    /// </summary>
    public string? CoorganizerEmail { get; set; }
}