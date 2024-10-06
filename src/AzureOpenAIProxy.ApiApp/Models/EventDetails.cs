using System.Text.Json.Serialization;

using Azure;
using Azure.Data.Tables;

/// <summary>
/// This represent the entity for the event details for users.
/// </summary>
public class EventDetails : ITableEntity
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

        /// <summary>
    /// Gets or sets the event start date.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset DateStart { get; set; }

    /// <summary>
    /// Gets or sets the event end date.
    /// </summary>
    [JsonRequired]
    public DateTimeOffset DateEnd { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public string PartitionKey { get; set; } = string.Empty;

    /// <inheritdoc />
    [JsonIgnore]
    public string RowKey { get; set; } = string.Empty;

    /// <inheritdoc />
    [JsonIgnore]
    public DateTimeOffset? Timestamp { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public ETag ETag { get; set; }
}