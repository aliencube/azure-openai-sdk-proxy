using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This represents the service entity for management authentication.
/// </summary>
/// <param name="client"><see cref="TableServiceClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
public class ManagementAuthService(TableServiceClient client, ILogger<ManagementAuthService> logger) : AuthService
{
    private const string TableName = "managements";
    private const string PartitionKeys = "master,management";

    private readonly TableClient _table = client?.GetTableClient(TableName) ?? throw new ArgumentNullException(nameof(client));
    private readonly ILogger<ManagementAuthService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public override async Task<T> ValidateAsync<T>(string apiKey)
    {
        var partitionKeys = PartitionKeys.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var now = DateTimeOffset.UtcNow;
        var results = this._table
                          .QueryAsync<ManagementRecord>(p => partitionKeys.Contains(p.PartitionKey)
                                                          && p.ApiKey == apiKey
                                                          && p.EventDateStart <= now && now <= p.EventDateEnd);

        var record = default(ManagementRecord);
        await foreach (var result in results.AsPages())
        {
            if (result.Values.Count != 1)
            {
                continue;
            }

            record = result.Values.Single();
            break;
        }

        return (T)Convert.ChangeType(record, typeof(T));
    }
}
