using AzureOpenAIProxy.ApiApp.Models;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;

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
public class AdminEventRepository : IAdminEventRepository
{
    private readonly string TableName = "events";
    private readonly TableServiceClient _tableServiceClient;

    public AdminEventRepository(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    public AdminEventRepository()
    {
        // TODO: [tae0y] TEST를 위한 임시 코드
    }


    /// <inheritdoc />
    public async Task<AdminEventDetails> CreateEvent(AdminEventDetails eventDetails)
    {
        //DONE: [tae0y] partition key : TimeZone / rowkey : Guid.NewGuid().ToString()
        //DONE: [tae0y] ITableEntity 상속/구현
        //TODO: [tae0y] table storage client 생성, 의존성 주입받는 방식이 이게 맞는가
        
        var tableServiceClient = _tableServiceClient.GetTableClient(TableName);

        eventDetails.PartitionKey = eventDetails.TimeZone;
        eventDetails.RowKey = eventDetails.EventId.ToString();
        var response = await tableServiceClient.AddEntityAsync(eventDetails);
        if (response.Status != 200)
        {
            throw new Exception("Failed to create event");
        }

        var addedEntity = await tableServiceClient.GetEntityAsync<AdminEventDetails>(eventDetails.PartitionKey, eventDetails.RowKey);
        return addedEntity;
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
