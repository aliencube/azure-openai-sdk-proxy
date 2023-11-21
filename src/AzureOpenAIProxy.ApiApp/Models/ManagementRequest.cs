using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the request entity for management.
/// </summary>
public class ManagementRequest
{
    /// <summary>
    /// Gets or sets the event name.
    /// </summary>
    [JsonPropertyName("eventName")]
    public string? EventName { get; set; }

    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    [JsonPropertyName("eventDescription")]
    public string? EventDescription { get; set; }

    /// <summary>
    /// Gets or sets the event organiser's name.
    /// </summary>
    [JsonPropertyName("eventOrganiser")]
    public string? EventOrganiser { get; set; }

    /// <summary>
    /// Gets or sets the event organiser's email address.
    /// </summary>
    [JsonPropertyName("eventOrganiserEmail")]
    public string? EventOrganiserEmail { get; set; }

    /// <summary>
    /// Gets or sets the event start date.
    /// </summary>
    [JsonPropertyName("eventDateStart")]
    public DateTimeOffset? EventDateStart { get; set; }

    /// <summary>
    /// Gets or sets the event end date.
    /// </summary>
    [JsonPropertyName("eventDateEnd")]
    public DateTimeOffset? EventDateEnd { get; set; }
}
