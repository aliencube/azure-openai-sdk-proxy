namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the admin event record.
/// </summary>
/// <param name="Title">Admin event title value.</param>
/// <param name="Summary">Admin event summary value.</param>
/// <param name="Description">Admin event description value.</param>
/// <param name="DateStart"><see cref="DateOnly"/>Admin event starting date value.</param>
/// <param name="DateEnd"><see cref="DateOnly"/>Admin event closing date value.</param>
/// <param name="TimeZone">Admin event timezone value.</param>
/// <param name="IsActive">Admin event active value.</param>
/// <param name="OrganizerName">Admin event organizer name value.</param>
/// <param name="OrganizerEmail">Admin event organizer email value.</param>
/// <param name="CoorganizerName">Admin event coorganizer name value.</param>
/// <param name="CoorganizerEmail">Admin event coorganizer email value.</param>
/// <param name="MaxTokenCap">Admin event max token value.</param>
/// <param name="DailyRequestCap">Admin event request capacity per daily value.</param>
public class AdminEvent
{
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