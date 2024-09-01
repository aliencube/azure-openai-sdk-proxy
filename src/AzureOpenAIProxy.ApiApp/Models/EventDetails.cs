/// <summary>
/// This represents the event's detailed data for response by EventEndpoint.
/// </summary>
public class EventDetails
{
    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the event title name.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event summary.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Azure OpenAI Service request max token capacity.
    /// </summary>
    public int MaxTokenCap { get; set; }

    /// <summary>
    /// Gets or sets the Azure OpenAI Service daily request capacity.
    /// </summary>
    public int DailyRequestCap { get; set; }
}