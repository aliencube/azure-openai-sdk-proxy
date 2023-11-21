using Azure;
using Azure.Data.Tables;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the table entity for access code.
/// </summary>
public class AccessCodeRecord : ITableEntity
{
    /// <inheritdoc />
    public string? PartitionKey { get; set; }

    /// <inheritdoc />
    public string? RowKey { get; set; }

    /// <summary>
    /// Gets or sets the event ID. It's equivalent to <see cref="AccessCodeRecord.PartitionKey"/>.
    /// </summary>
    public string? EventId { get; set; }

    /// <summary>
    /// Gets or sets the access code ID. It's equivalent to <see cref="AccessCodeRecord.RowKey"/>.
    /// </summary>
    public string AccessCodeId { get; set; }

    /// <summary>
    /// Gets or sets the name who holds the access code.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the email address who holds the access code.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the GitHub alias who holds the access code.
    /// </summary>
    public string? GitHubAlias { get; set; }

    /// <summary>
    /// Gets or sets the access code. It's equivalent to <see cref="AccessCodeRecord.RowKey"/>
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    public DateTimeOffset? DateCreated { get; set; }

    /// <summary>
    /// Gets or sets the event date start.
    /// </summary>
    public DateTimeOffset? EventDateStart { get; set; }

    /// <summary>
    /// Gets or sets the event date end.
    /// </summary>
    public DateTimeOffset? EventDateEnd { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens to generate.
    /// </summary>
    public int MaxTokens { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? Timestamp { get; set; }

    /// <inheritdoc />
    public ETag ETag { get; set; }
}
