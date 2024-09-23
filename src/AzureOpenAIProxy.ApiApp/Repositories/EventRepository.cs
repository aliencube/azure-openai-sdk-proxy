namespace AzureOpenAIProxy.ApiApp.Repositories;

/// <summary>
/// This provides interfaces to the <see cref="EventRepository"/> class.
/// </summary>
public interface IEventRepository
{
    /// <summary>
    /// Gets the list of events.
    /// </summary>
    /// <returns>Returns the list of events.</returns>
    Task<List<EventDetails>> GetEvents();
}

public class EventRepository() : IEventRepository
{
    /// <inheritdoc/>
    public async Task<List<EventDetails>> GetEvents()
    {
        throw new NotImplementedException();
    }
}