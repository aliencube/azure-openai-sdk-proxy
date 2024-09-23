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
    public async Task<List<EventDetails>> GetEvents()
    {
        TableClient tableClient = await GetTableClientAsync();

        List<EventDetails> events = [];

        await foreach(EventDetails eventDetails in tableClient.QueryAsync<EventDetails>(e => e.PartitionKey.Equals(PartitionKeys.EventDetails)))
        {
            events.Add(eventDetails);
        }

        return events;
    }

    private async Task<TableClient> GetTableClientAsync()
    {
        TableClient tableClient = _tableServiceClient.GetTableClient(_storageAccountSettings.TableStorage.TableName);

        await tableClient.CreateIfNotExistsAsync().ConfigureAwait(false);

        return tableClient;
    }
}