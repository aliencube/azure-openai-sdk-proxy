namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the response entity collection for event.
/// </summary>
public class EventResponseCollection
{
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int? CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public int? Total { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="EventResponse"/> instances.
    /// </summary>
    public List<EventResponse> Items { get; set; } = [];
}
