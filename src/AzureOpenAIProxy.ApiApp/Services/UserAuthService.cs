using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This represents the service entity for user authentication.
/// </summary>
/// <param name="client"><see cref="TableServiceClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
public class UserAuthService(TableServiceClient client, ILogger<UserAuthService> logger) : AuthService(client, logger)
{
    private const string TableName = "accesscodes";

    private readonly TableClient _table = client.GetTableClient(TableName);

    /// <inheritdoc />
    public override async Task<bool> ValidateAsync(string apiKey)
    {
        var segments = apiKey.Split("::");
        var accessCode = segments[0];
        var githubAlias = segments[1];
        var results = this._table
                          .QueryAsync<AccessCodeRecord>(p =>
                                                        p.RowKey == accessCode &&
                                                        p.GitHubAlias.Equals(githubAlias, StringComparison.InvariantCultureIgnoreCase));

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