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
}