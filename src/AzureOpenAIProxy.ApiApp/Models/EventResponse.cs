using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the response entity for event.
/// </summary>
public class EventResponse : EventRequest, IEntityResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventResponse"/> class.
    /// </summary>
    public EventResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventResponse"/> class.
    /// </summary>
    /// <param name="record"><see cref="EventRecord"/> instance.</param>
    public EventResponse(EventRecord record)
    {
        this.Id = record.EventId;
        this.AccessCode = record.ApiKey;
        this.EventName = record.EventName;
        this.EventDescription = record.EventDescription;
        this.EventOrganiser = record.EventOrganiser;
        this.EventOrganiserEmail = record.EventOrganiserEmail;
        this.EventDateStart = record.EventDateStart;
        this.EventDateEnd = record.EventDateEnd;
    }

    /// <summary>
    /// Gets or sets the event ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the event access code to the API.
    /// </summary>
    [JsonPropertyName("accessCode")]
    public string? AccessCode { get; set; }
}
