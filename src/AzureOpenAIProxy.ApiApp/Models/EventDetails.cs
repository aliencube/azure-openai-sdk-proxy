using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the event's detailed data for response by EventEndpoint.
/// </summary>
public class EventDetails
{
    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    [JsonPropertyName("eventId")]
    public required string? EventId { get; set; }

    /// <summary>
    /// Gets or sets the event title.
    /// </summary>
    [JsonPropertyName("title")]
    public required string? Title { get; set; }

    /// <summary>
    /// Gets or sets the event summary.
    /// </summary>
    [JsonPropertyName("summary")]
    public required string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the event's start time.
    /// </summary>
    [JsonPropertyName("dateStart")]
    public required DateTimeOffset? DateStart { get; set; }

    /// <summary>
    /// Gets or sets the event's end time.
    /// </summary>
    [JsonPropertyName("dateEnd")]
    public required DateTimeOffset? DateEnd { get; set; }

    /// <summary>
    /// Gets or sets the event's time zone.
    /// </summary>
    [JsonPropertyName("tz")]
    public required string? TimeZone { get; set; }

    /// <summary>
    /// Gets or sets the event's active status.
    /// </summary>
    [JsonPropertyName("isActive")]
    public required bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the organizer's name.
    /// </summary>
    [JsonPropertyName("organizerName")]
    public required string? OrganizerName { get; set; }

    /// <summary>
    /// Gets or sets the organizer's email address.
    /// </summary>
    [EmailAddress]
    [JsonPropertyName("OrganizerEmail")]
    public required string? OrganizerEmail { get; set; }

    /// <summary>
    /// Gets or sets the co-organizer's name.
    /// </summary>
    [JsonPropertyName("CoorganizerName")]
    public string? CoorganizerName { get; set; }

    /// <summary>
    /// Gets or sets the co-organizer's email address.
    /// </summary>
    [JsonPropertyName("CoorganizerEmail")]
    public string? CoorganizerEmail { get; set; }

    /// <summary>
    /// Gets or sets the event's maximum token capacity.
    /// </summary>
    [JsonPropertyName("maxTokenCap")]
    public required int? MaxTokenCap { get; set; }

    /// <summary>
    /// Gets or sets the event's daily request capacity.
    /// </summary>
    [JsonPropertyName("dailyRequestCap")]
    public required int? DailyRequestCap { get; set; }
}