using System.Text.Json.Serialization;

using Azure;
using Azure.Data.Tables;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represent the entity for the event details for admin.
/// </summary>
public class AdminEventDetails : EventDetails, ITableEntity
{
    /// <summary>
    /// Gets or sets the event description.
    /// </summary>
    public string? Description { get; set; }

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

    /// <summary>
    /// Gets or sets the event start to end date timezone.
    /// </summary>
    [JsonRequired]
    public string TimeZone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event active status.
    /// </summary>
    [JsonRequired]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the event organizer name.
    /// </summary>
    [JsonRequired]
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event organizer email.
    /// </summary>
    [JsonRequired]
    public string OrganizerEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event coorganizer name.
    /// </summary>
    public string? CoorganizerName { get; set; }

    /// <summary>
    /// Gets or sets the event coorganizer email.
    /// </summary>
    public string? CoorganizerEmail { get; set; }

    // ITableEntity implementation
    /// <summary>
    /// Gets or sets the event Partition Key.
    /// </summary>
    public string? PartitionKey { get; set; }
    
    /// <summary>
    /// Gets or sets the event Row Key.
    /// </summary>
    public string? RowKey { get; set; }
    
    /// <summary>
    /// Gets or sets the event Timestamp.
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }
    
    /// <summary>
    /// Gets or sets the event ETag.
    /// </summary>
    public ETag ETag { get; set; }
}