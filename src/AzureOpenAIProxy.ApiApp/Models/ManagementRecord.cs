using Azure;
using Azure.Data.Tables;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the table entity for management.
/// </summary>
public class ManagementRecord : ITableEntity
{
    /// <inheritdoc />
    public string? PartitionKey { get; set; }

    /// <inheritdoc />
    public string? RowKey { get; set; }

    /// <summary>
    /// Gets or sets the event ID. It's equivalent to <see cref="ManagementRecord.RowKey"/>.
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
    /// Gets or sets the event start date.
    /// </summary>
    public DateTimeOffset? EventDateStart { get; set; }

    /// <summary>
    /// Gets or sets the event end date.
    /// </summary>
    public DateTimeOffset? EventDateEnd { get; set; }

    /// <summary>
    /// Gets or sets the management API key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? Timestamp { get; set; }

    /// <inheritdoc />
    public ETag ETag { get; set; }
}
