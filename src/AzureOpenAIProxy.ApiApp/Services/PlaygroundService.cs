using AzureOpenAIProxy.ApiApp.Repositories;

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
    Task<List<DeploymentModelDetails>> GetDeploymentModels(Guid eventId);

    /// <summary>
    /// Get the list of events.
    /// </summary>
    /// <returns>Returns the list of events.</returns>
    Task<List<EventDetails>> GetEvents();
}

public class PlaygroundService(IEventRepository eventRepository) : IPlaygroundService
{
    private readonly IEventRepository _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
    
    /// <inheritdoc/>
    public async Task<List<DeploymentModelDetails>> GetDeploymentModels(Guid eventId)
    {
        var result = await _eventRepository.GetDeploymentModels(eventId).ConfigureAwait(false);
        
        return result;
    }

    /// <inheritdoc/>
    public async Task<List<EventDetails>> GetEvents()
    {
        var result = await _eventRepository.GetEvents().ConfigureAwait(false);

        return result;
    }
}

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class PlaygroundServiceExtensions
{
    /// <summary>
    /// Adds the <see cref="PlaygroundService"/> instance to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddPlaygroundService(this IServiceCollection services)
    {
        services.AddScoped<IPlaygroundService, PlaygroundService>();

        return services;
    }
}