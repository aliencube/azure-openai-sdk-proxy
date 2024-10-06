using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Configurations;

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

public class EventRepository(TableServiceClient tableServiceClient, StorageAccountSettings storageAccountSettings) : IEventRepository
{
    private readonly TableServiceClient _tableServiceClient = tableServiceClient ?? throw new ArgumentNullException(nameof(tableServiceClient));
    private readonly StorageAccountSettings _storageAccountSettings = storageAccountSettings ?? throw new ArgumentNullException(nameof(storageAccountSettings));

    /// <inheritdoc/>
    /// <remarks>
    /// The results are sorted based on the following criteria:
    /// 1. Lexical order of event titles.
    /// 2. Start date of event.
    /// 3. End date of event.
    /// </remarks>
    public async Task<List<EventDetails>> GetEvents()
    {
        TableClient tableClient = await GetTableClientAsync();

        List<EventDetails> events = [];

        await foreach(EventDetails eventDetails in tableClient.QueryAsync<EventDetails>(e => e.PartitionKey.Equals(PartitionKeys.EventDetails)).ConfigureAwait(false))
        {
            events.Add(eventDetails);
        }

        events.Sort((e1, e2) => 
        {
            if(!e1.Title.Equals(e2.Title))
            {
                return e1.Title.CompareTo(e2.Title);
            }

            if(!e1.DateStart.Equals(e2.DateStart))
            {
                return e1.DateStart.CompareTo(e2.DateStart);
            }

            return e1.DateEnd.CompareTo(e2.DateEnd);
        });

        return events;
    }

    private async Task<TableClient> GetTableClientAsync()
    {
        TableClient tableClient = _tableServiceClient.GetTableClient(_storageAccountSettings.TableStorage.TableName);

        await tableClient.CreateIfNotExistsAsync().ConfigureAwait(false);

        return tableClient;
    }
}

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class EventRepositoryExtensions
{
    /// <summary>
    /// Adds the <see cref="EventRepository"/> instance to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddEventRepository(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();

        return services;
    }
}