using Azure;
using Azure.Data.Tables;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the table entity for event.
/// </summary>
public class EventRecord : ITableEntity
{
    /// <inheritdoc />
    public string? PartitionKey { get; set; }

    /// <inheritdoc />
    public string? RowKey { get; set; }

    /// <summary>
    /// Gets or sets the event ID. It's equivalent to <see cref="EventRecord.RowKey"/>.
    /// </summary>
    public string? EventId { get; set; }

    /// <summary>
    /// Gets or sets the event name.
    /// </summary>
    public string? EventName { get; set; }

    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    public string? EventDescription { get; set; }

    /// <summary>
    /// Gets or sets the event organiser's name.
    /// </summary>
    public string? EventOrganiser { get; set; }

    /// <summary>
    /// Gets or sets the event organiser's email address.
    /// </summary>
    public string? EventOrganiserEmail { get; set; }

    /// <summary>
    /// Gets or sets the event start date in UTC.
    /// </summary>
    public DateTimeOffset? EventDateStart { get; set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// Gets or sets the event end date in UTC.
    /// </summary>
    public DateTimeOffset? EventDateEnd { get; set; } = DateTimeOffset.MaxValue;

    /// <summary>
    /// Gets or sets the management API key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens for the event. Defaults to 4096.
    /// </summary>
    public int? MaxTokens { get; set; } = 4096;

    /// <inheritdoc />
    public DateTimeOffset? Timestamp { get; set; }

    /// <inheritdoc />
    public ETag ETag { get; set; }
}
