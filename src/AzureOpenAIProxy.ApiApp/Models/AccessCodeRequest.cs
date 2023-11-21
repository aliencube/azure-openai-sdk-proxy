using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the request entity for access code.
/// </summary>
public class AccessCodeRequest
{
    /// <summary>
    /// Gets or sets the event ID.
    /// </summary>
    [JsonPropertyName("eventId")]
    public string? EventId { get; set; }

    /// <summary>
    /// Gets or sets the name who requests the access code.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the email address who requests the access code.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the GitHub alias who requests the access code.
    /// </summary>
    [JsonPropertyName("gitHubAlias")]
    public string? GitHubAlias { get; set; }

    /// <summary>
    /// Gets or sets the date when the event starts.
    /// </summary>
    [JsonPropertyName("eventDateStart")]
    public DateTimeOffset? EventDateStart { get; set; }

    /// <summary>
    /// Gets or sets the date when the event ends.
    /// </summary>
    [JsonPropertyName("eventDateEnd")]
    public DateTimeOffset? EventDateEnd { get; set; }

    /// <summary>
    /// Gets or sets the date when the access code is requested.
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTimeOffset? DateCreated { get; set; }
}
