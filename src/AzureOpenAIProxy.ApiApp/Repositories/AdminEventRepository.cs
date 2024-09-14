using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Repositories;

/// <summary>
/// This provides interfaces to the <see cref="AdminEventRepository"/> class.
/// </summary>
public interface IAdminEventRepository
{
    /// <summary>
    /// Creates a new record of event details.
    /// </summary>
    /// <param name="eventDetails">Event details instance.</param>
    /// <returns>Returns the event details instance created.</returns>
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
    /// <returns>Returns the event details record.</returns>
    Task<AdminEventDetails> GetEvent(Guid eventId);

    /// <summary>
    /// Updates the event details.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <param name="eventDetails">Event details instance.</param>
    /// <returns>Returns the updated record of the event details.</returns>
    Task<AdminEventDetails> UpdateEvent(Guid eventId, AdminEventDetails eventDetails);
}

/// <summary>
/// This represents the repository entity for the admin event.
/// </summary>
public class AdminEventRepository(TableServiceClient tableServiceClient) : IAdminEventRepository
{
    private readonly TableServiceClient _tableServiceClient = tableServiceClient ?? throw new ArgumentNullException(nameof(tableServiceClient));

    /// <inheritdoc />
    public async Task<AdminEventDetails> CreateEvent(AdminEventDetails eventDetails)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<List<AdminEventDetails>> GetEvents()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<AdminEventDetails> GetEvent(Guid eventId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<AdminEventDetails> UpdateEvent(Guid eventId, AdminEventDetails eventDetails)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class AdminEventRepositoryExtensions
{
    /// <summary>
    /// Adds the <see cref="AdminEventRepository"/> instance to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAdminEventRepository(this IServiceCollection services)
    {
        services.AddScoped<IAdminEventRepository, AdminEventRepository>();

        return services;
    }
}
