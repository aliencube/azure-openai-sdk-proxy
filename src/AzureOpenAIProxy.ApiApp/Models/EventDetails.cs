using System.Text.Json.Serialization;

/// <summary>
/// This represents the event's detailed data for response by EventEndpoint.
/// </summary>
public class EventDetails
{
    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    [JsonRequired]
    public Guid EventId { get; set; }

    /// <summary>
    /// Gets or sets the event title name.
    /// </summary>
    [JsonRequired]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event summary.
    /// </summary>
    [JsonRequired]
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Azure OpenAI Service request max token capacity.
    /// </summary>
    [JsonRequired]
    public int MaxTokenCap { get; set; }

    /// <summary>
    /// Gets or sets the Azure OpenAI Service daily request capacity.
    /// </summary>
    [JsonRequired]
    public int DailyRequestCap { get; set; }
}
