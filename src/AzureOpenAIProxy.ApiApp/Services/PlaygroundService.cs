namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to <see cref="PlaygroundService"/> class. 
/// </summary>
public interface IPlaygroundService
{
    /// <summary>
    /// Get the list of deployment model.
    /// </summary>
    /// <returns>Returns the list of deployment models.</returns>
    Task<List<DeploymentModelDetails>> GetDeploymentModels(string eventId);

    /// <summary>
    /// Get the list of events.
    /// </summary>
    /// <returns>Returns the list of events.</returns>
    Task<List<EventDetails>> GetEvents();
}

public class PlaygroundService() : IPlaygroundService
{
    /// <inheritdoc/>
    public async Task<List<DeploymentModelDetails>> GetDeploymentModels(string eventId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<List<EventDetails>> GetEvents()
    {
        throw new NotImplementedException();
    }
}