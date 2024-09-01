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
    public required DateTimeOffset? DateStart { get; set; }

    /// <summary>
    /// Gets or sets the event end date.
    /// </summary>
    public required DateTimeOffset? DateEnd { get; set; }

    /// <summary>
    /// Gets or sets the event start to end date timezone.
    /// </summary>
    public required string? TimeZone { get; set; }

    /// <summary>
    /// Gets or sets the event active status.
    /// </summary>
    public required bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the event organizer name.
    /// </summary>
    public required string? OrganizerName { get; set; }

    /// <summary>
    /// Gets or sets the event organizer email.
    /// </summary>
    public required string? OrganizerEmail { get; set; }

    /// <summary>
    /// Gets or sets the event coorganizer name.
    /// </summary>
    public string? CoorganizerName { get; set; }

    /// <summary>
    /// Gets or sets the event coorganizer email.
    /// </summary>
    public string? CoorganizerEmail { get; set; }
}