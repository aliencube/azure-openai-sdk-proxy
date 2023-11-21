using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This represents the service entity for user authentication.
/// </summary>
/// <param name="client"><see cref="TableServiceClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
public class UserAuthService(TableServiceClient client, ILogger<UserAuthService> logger) : AuthService
{
    private const string TableName = "accesscodes";

    private readonly TableClient _table = client?.GetTableClient(TableName) ?? throw new ArgumentNullException(nameof(client));
    private readonly ILogger<UserAuthService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public override async Task<bool> ValidateAsync(string apiKey)
    {
        var segments = apiKey.Split("::");
        var accessCode = segments[0];
        var githubAlias = segments[1];
        var now = DateTimeOffset.UtcNow;
        var results = this._table
                          .QueryAsync<AccessCodeRecord>(p => p.RowKey == accessCode
                                                          && p.GitHubAlias.Equals(githubAlias, StringComparison.InvariantCultureIgnoreCase)
                                                          && p.EventDateStart <= now && now <= p.EventDateEnd);

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