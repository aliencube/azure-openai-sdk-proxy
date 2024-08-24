namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represent the event detail data for response by admin event endpoint.
/// </summary>
public class AdminEventDetails
{
    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    public Guid? EventId { get; set; }

    /// <summary>
    /// Gets or sets the event title name.
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

    /// <summary>
    /// Gets or sets the Azure OpenAI Service request max token capacity. 
    /// </summary>
    public required int? MaxTokenCap { get; set; }

    /// <summary>
    /// Gets or sets the Azure OpenAI Service daily request capacity.
    /// </summary>
    public required int? DailyRequestCap { get; set; }
}