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
    public required string? EventId { get; set; }

    /// <summary>
    /// Gets or sets the event title.
    /// </summary>
    public required string? Title { get; set; }

    /// <summary>
    /// Gets or sets the event summary.
    /// </summary>
    public required string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the event's start time.
    /// </summary>
    public required DateTimeOffset? DateStart { get; set; }

    /// <summary>
    /// Gets or sets the event's end time.
    /// </summary>
    public required DateTimeOffset? DateEnd { get; set; }

    /// <summary>
    /// Gets or sets the event's time zone.
    /// </summary>
    public required string? TimeZone { get; set; }

    /// <summary>
    /// Gets or sets the event's active status.
    /// </summary>
    public required bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the organizer's name.
    /// </summary>
    public required string? OrganizerName { get; set; }

    /// <summary>
    /// Gets or sets the organizer's email address.
    /// </summary>
    public required string? OrganizerEmail { get; set; }

    /// <summary>
    /// Gets or sets the co-organizer's name.
    /// </summary>
    public string? CoorganizerName { get; set; }

    /// <summary>
    /// Gets or sets the co-organizer's email address.
    /// </summary>
    public string? CoorganizerEmail { get; set; }

    /// <summary>
    /// Gets or sets the event's maximum token capacity.
    /// </summary>
    public required int? MaxTokenCap { get; set; }

    /// <summary>
    /// Gets or sets the event's daily request capacity.
    /// </summary>
    public required int? DailyRequestCap { get; set; }
}