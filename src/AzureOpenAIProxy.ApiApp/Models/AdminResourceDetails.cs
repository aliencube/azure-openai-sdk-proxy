using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Azure;
using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Converters;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represent the entity for the resource details for admin.
/// </summary>
public class AdminResourceDetails : ITableEntity
{
    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    [JsonRequired]
    public Guid ResourceId { get; set; }

    /// <summary>
    /// Gets or sets the friendly name of the resource.
    /// </summary>
    [JsonRequired]
    public string FriendlyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deployment name of the resource.
    /// </summary>
    [JsonRequired]
    public string DeploymentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource type.
    /// </summary>
    [JsonRequired]
    public ResourceType ResourceType { get; set; } = ResourceType.None;

    /// <summary>
    /// Gets or sets the resource endpoint.
    /// </summary>
    [JsonRequired]
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource API key.
    /// </summary>
    [JsonRequired]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource region.
    /// </summary>
    [JsonRequired]
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value indicating whether the resource is active.
    /// </summary>
    [JsonRequired]
    public bool IsActive { get; set; }

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

/// <summary>
/// This defines the type of the resource.
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<ResourceType>))]
public enum ResourceType
{
    /// <summary>
    /// Indicates the resource type is not defined.
    /// </summary>
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Indicates the chat resource type.
    /// </summary>
    [EnumMember(Value = "chat")]
    Chat,

    /// <summary>
    /// Indicates the image resource type.
    /// </summary>
    [EnumMember(Value = "image")]
    Image,
}