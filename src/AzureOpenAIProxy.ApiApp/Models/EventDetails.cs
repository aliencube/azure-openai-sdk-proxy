using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the event's detailed data for response by EventEndpoint.
/// </summary>
public class EventDetails
{
    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    public required string? EventId { get; set; }

    /// <summary>
    /// Gets or sets the event title.
    /// </summary>
    public required string? Title { get; set; }

    /// <summary>
    /// Gets or sets the event summary.
    /// </summary>
    public required string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the event's maximum token capacity.
    /// </summary>
    public required int? MaxTokenCap { get; set; }

    /// <summary>
    /// Gets or sets the event's daily request capacity.
    /// </summary>
    public required int? DailyRequestCap { get; set; }
}