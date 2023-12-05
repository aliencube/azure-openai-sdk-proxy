using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This represents the service entity for management authentication.
/// </summary>
/// <param name="client"><see cref="TableServiceClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
public class ManagementAuthService(TableServiceClient client, ILogger<ManagementAuthService> logger) : AuthService<EventRecord>
{
    private const string TableName = "managements";
    private const string PartitionKeyMaster = "master";
    private const string PartitionKeyManagement = "management";

    private readonly TableClient _table = client?.GetTableClient(TableName) ?? throw new ArgumentNullException(nameof(client));
    private readonly ILogger<ManagementAuthService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public override async Task<EventRecord> ValidateAsync(string apiKey)
    {
        var results = this._table
                          .QueryAsync<EventRecord>(p => (p.PartitionKey.Equals(PartitionKeyMaster, StringComparison.InvariantCultureIgnoreCase) ||
                                                         p.PartitionKey.Equals(PartitionKeyManagement, StringComparison.InvariantCultureIgnoreCase))
                                                     && p.ApiKey == apiKey);

        var record = default(EventRecord);
        await foreach (var result in results.AsPages())
        {
            if (result.Values.Count != 1)
            {
                continue;
            }

            record = result.Values.Single();
            break;
        }

        var now = DateTimeOffset.UtcNow;

        if (record.PartitionKey.Equals(PartitionKeyMaster, StringComparison.InvariantCultureIgnoreCase))
        {
            return record;
        }

        if (record.EventDateStart <= now && now < record.EventDateEnd)
        {
            return record;
        }

        return default(EventRecord);
    }
}
