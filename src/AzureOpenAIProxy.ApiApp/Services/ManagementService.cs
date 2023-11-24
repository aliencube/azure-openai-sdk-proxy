using System.Text.Json;

using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="ManagementService"/> class.
/// </summary>
public interface IManagementService
{
    /// <summary>
    /// Creates the access code.
    /// </summary>
    /// <param name="req"><see cref="AccessCodeRequest"/> instance.</param>
    /// <returns>Returns the <see cref="AccessCodeResponse"/> instance.</returns>
    Task<AccessCodeResponse> CreateAccessCodeAsync(AccessCodeRequest req);
}

/// <summary>
/// This represents the service entity for management.
/// </summary>
/// <param name="client"><see cref="TableServiceClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
public class ManagementService(TableServiceClient client, ILogger<ManagementService> logger) : IManagementService
{
    private const string AccessCodesTableName = "accesscodes";
    private const string ManagementsTableName = "managements";
    private const string ManagementsTablePartitionKey = "management";

    private readonly TableClient _accessCodes = client?.GetTableClient(AccessCodesTableName) ?? throw new ArgumentNullException(nameof(client));
    private readonly TableClient _managements = client?.GetTableClient(ManagementsTableName) ?? throw new ArgumentNullException(nameof(client));
    private readonly ILogger<ManagementService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<AccessCodeResponse> CreateAccessCodeAsync(AccessCodeRequest req)
    {
        var @event = await this._managements
                               .GetEntityIfExistsAsync<ManagementRecord>(ManagementsTablePartitionKey, req.EventId)
                           ?? throw new KeyNotFoundException($"Event ID not found: {req.EventId}");

        var apiKey = Guid.NewGuid().ToString();
        var record = new AccessCodeRecord()
        {
            PartitionKey = req.EventId,
            RowKey = apiKey,
            EventId = req.EventId,
            Name = req.Name,
            Email = req.Email,
            GitHubAlias = req.GitHubAlias,
            ApiKey = apiKey,
            EventDateStart = @event?.Value?.EventDateStart,
            EventDateEnd = @event?.Value?.EventDateEnd,
            DateCreated = DateTimeOffset.UtcNow,
        };

        var response = await this._accessCodes.UpsertEntityAsync(record);
        using var reader = new StreamReader(response.ContentStream);
        var payload = await reader.ReadToEndAsync();
        var result = JsonSerializer.Deserialize<AccessCodeRecord>(payload);

        return new AccessCodeResponse(result);
    }
}
