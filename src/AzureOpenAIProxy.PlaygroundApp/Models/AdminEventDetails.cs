using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.PlaygroundApp.Models;

/// <summary>
/// This represent the event detail data for response by admin event endpoint.
/// </summary>
public class AdminEventDetails : EventDetails
{
    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the event start date.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset DateStart { get; set; }

    /// <summary>
    /// Gets or sets the event end date.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset DateEnd { get; set; }

    /// <summary>
    /// Gets or sets the event start to end date timezone.
    /// </summary>
    [JsonRequired]
    public string TimeZone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event active status.
    /// </summary>
    [JsonRequired]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the event organizer name.
    /// </summary>
    [JsonRequired]
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event organizer email.
    /// </summary>
    [JsonRequired]
    public string OrganizerEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event coorganizer name.
    /// </summary>
    public string? CoorganizerName { get; set; }

    /// <summary>
    /// Gets or sets the event coorganizer email.
    /// </summary>
    public string? CoorganizerEmail { get; set; }
}