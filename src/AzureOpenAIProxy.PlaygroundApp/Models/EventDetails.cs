namespace AzureOpenAIProxy.PlaygroundApp.Models;

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
    /// Gets or sets the event title name.
    /// </summary>
    public required string? Title { get; set; }

    /// <summary>
    /// Gets or sets the event summary.
    /// </summary>
    public required string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the Azure OpenAI Service request max token capacity.
    /// </summary>
    public required int? MaxTokenCap { get; set; }

    /// <summary>
    /// Gets or sets the Azure OpenAI Service daily request capacity.
    /// </summary>
    public required int? DailyRequestCap { get; set; }
}