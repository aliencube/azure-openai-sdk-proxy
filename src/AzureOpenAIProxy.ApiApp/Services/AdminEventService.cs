using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to <see cref="AdminEventService"/> class.
/// </summary>
public interface IAdminEventService
{
    /// <summary>
    /// Creates a new event.
    /// </summary>
    /// <param name="eventDetails">Event payload.</param>
    /// <returns>Returns the event payload created.</returns>
    Task<AdminEventDetails> CreateEvent(AdminEventDetails eventDetails);

    /// <summary>
    /// Gets the list of events.
    /// </summary>
    /// <returns>Returns the list of events.</returns>
    Task<List<AdminEventDetails>> GetEvents();

    /// <summary>
    /// Gets the event details.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <returns>Returns the event details.</returns>
    Task<AdminEventDetails> GetEvent(Guid eventId);

    /// <summary>
    /// Updates the event details.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <param name="eventDetails">Event details to update.</param>
    /// <returns>Returns the updated event details.</returns>
    Task<AdminEventDetails> UpdateEvent(Guid eventId, AdminEventDetails eventDetails);
}

/// <summary>
/// This represents the service entity for admin event.
/// </summary>
public class AdminEventService(IAdminEventRepository repository) : IAdminEventService
{
    private readonly IAdminEventRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <inheritdoc />
    public async Task<AdminEventDetails> CreateEvent(AdminEventDetails eventDetails)
    {
        // Validate
        eventDetails.PartitionKey = PartitionKeys.EventDetails;
        eventDetails.RowKey = eventDetails.EventId.ToString();
        
        // Save
        var response = await this._repository.CreateEvent(eventDetails).ConfigureAwait(false);
        return response;
    }

    /// <inheritdoc />
    public async Task<List<AdminEventDetails>> GetEvents()
    {
        var result = await this._repository.GetEvents().ConfigureAwait(false);

        return result;
    }

    /// <inheritdoc />
    public async Task<AdminEventDetails> GetEvent(Guid eventId)
    {
        var result = await this._repository.GetEvent(eventId).ConfigureAwait(false);

        return result;
    }

    /// <inheritdoc />
    public async Task<AdminEventDetails> UpdateEvent(Guid eventId, AdminEventDetails eventDetails)
    {
        var result = await this._repository.UpdateEvent(eventId, eventDetails).ConfigureAwait(false);

        return result;
    }
}

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class AdminEventServiceExtensions
{
    /// <summary>
    /// Adds the <see cref="AdminEventService"/> instance to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAdminEventService(this IServiceCollection services)
    {
        services.AddScoped<IAdminEventService, AdminEventService>();

        return services;
    }
}
