namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This provides interfaces to the response entity classes.
/// </summary>
public interface IEntityResponse
{
    /// <summary>
    /// Gets or sets the entity ID.
    /// </summary>
    string? Id { get; set; }
}

/// <summary>
/// This represents the response entity collection. This MUST be inherited.
/// </summary>
/// <typeparam name="T">Type of response</typeparam>
public abstract class EntityResponseCollection<T> where T : IEntityResponse
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
    /// Gets or sets the list of <see cref="T"/> instances.
    /// </summary>
    public List<T> Items { get; set; } = [];
}