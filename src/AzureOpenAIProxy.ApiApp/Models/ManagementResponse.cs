using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the response entity for management.
/// </summary>
public class ManagementResponse : ManagementRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementResponse"/> class.
    /// </summary>
    public ManagementResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementResponse"/> class.
    /// </summary>
    /// <param name="record"><see cref="ManagementRecord"/> instance.</param>
    public ManagementResponse(ManagementRecord record)
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
