using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// Data transfer model for showing all events
/// </summary>
public class EventListEntity
{
    /// <summary>
    /// Event Id
    /// </summary>
    [JsonPropertyName("eventId")]
    public required string EventId { get; set; }

    /// <summary>
    /// Event name
    /// </summary>
    [JsonPropertyName("eventName")]
    public required string EventName { get; set; }

    /// <summary>
    ///  Event start time
    /// </summary>
    [JsonPropertyName("startTime")]
    public required DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// Event end time
    /// </summary>
    [JsonPropertyName("endTime")]
    public required DateTimeOffset? EndTime { get; set; }

    /// <summary>
    /// Event's time zone. It could be serialized to string
    /// </summary>
    [JsonPropertyName("tz")]
    public required string? TimeZone { get; set; }

    /// <summary>
    /// Organizer's name
    /// </summary>
    [JsonPropertyName("organizerName")]
    public required string OrganizerName { get; set; }

    /// <summary>
    /// Organizer's email address.
    /// </summary>
    [EmailAddress]
    [JsonPropertyName("OrganizerEmail")]
    public required string OrganizerEmail { get; set; }

    /// <summary>
    /// The number of registered users
    /// </summary>
    [JsonPropertyName("registered")]
    public required int Registered { get; set; }

    /// <summary>
    /// List of resources that the organizer deploys
    /// </summary>
    [JsonPropertyName("resources")]
    public virtual IEnumerable<ResourceEntity>? Resources { get; set; }

    /// <summary>
    /// Shows whether the event is active or not
    /// </summary>
    [JsonPropertyName("isActive")]
    public required bool IsActive { get; set; }

    /// <summary>
    ///  Define the list entity of Resources
    /// </summary>
    public class ResourceEntity
    {
        /// <summary>
        /// Resource's Id
        /// </summary>
        [JsonPropertyName("id")]
        public required string ResourceId {get; set; }

        /// <summary>
        /// Resource's name. e.g. gpt4o
        /// </summary>
        [JsonPropertyName("name")]
        public required string ResourceName {get; set; }
    }
}