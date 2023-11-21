using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This represents the service entity for management authentication.
/// </summary>
/// <param name="client"><see cref="TableServiceClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
public class ManagementAuthService(TableServiceClient client, ILogger<ManagementAuthService> logger) : AuthService(client, logger)
{
    private const string TableName = "managements";
    private const string PartitionKey = "management";

    private readonly TableClient _table = client.GetTableClient(TableName);

    /// <inheritdoc />
    public override async Task<bool> ValidateAsync(string apiKey)
    {
        var results = this._table
                          .QueryAsync<ManagementRecord>(p => p.PartitionKey == PartitionKey && p.ApiKey == apiKey);

        var authenticated = false;
        await foreach (var result in results.AsPages())
        {
            if (result.Values.Count != 1)
            {
                continue;
            }

            authenticated = true;
            break;
        }

        return authenticated;
    }
}
