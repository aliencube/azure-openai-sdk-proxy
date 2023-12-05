using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="ManagementService"/> class.
/// </summary>
public partial interface IManagementService
{
    /// <summary>
    /// Gets the list of access codes by event ID.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <param name="page">Page number.</param>
    /// <param name="size">Page size.</param>
    /// <returns>Returns the <see cref="AccessCodeResponseCollection"/> instance.</returns>
    Task<AccessCodeResponseCollection> GetAccessCodesAsync(string eventId, int? page = 0, int? size = 20);

    /// <summary>
    /// Creates the access code.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <param name="req"><see cref="AccessCodeRequest"/> instance.</param>
    /// <returns>Returns the <see cref="AccessCodeResponse"/> instance.</returns>
    Task<AccessCodeResponse> CreateAccessCodeAsync(string eventId, AccessCodeRequest req);

    /// <summary>
    /// Gets the access code by GitHub alias.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <param name="gitHubAlias">GitHub alias.</param>
    /// <returns>Returns the <see cref="AccessCodeResponse"/> instance.</returns>
    Task<AccessCodeResponse> GetAccessCodeByGitHubAliasAsync(string eventId, string gitHubAlias);
}

/// <summary>
/// This represents the service entity for management.
/// </summary>
public partial class ManagementService
{
    /// <inheritdoc />
    public async Task<AccessCodeResponseCollection> GetAccessCodesAsync(string eventId, int? page = 0, int? size = 20)
    {
        if (string.IsNullOrWhiteSpace(eventId))
        {
            throw new ArgumentNullException(nameof(eventId));
        }

        var results = this._accessCodes.QueryAsync<AccessCodeRecord>(p => p.PartitionKey
                                                                           .Equals(eventId, StringComparison.InvariantCultureIgnoreCase));
        var records = new List<AccessCodeResponse>();
        await foreach (var result in results.AsPages())
        {
            records.AddRange(result.Values.Select(p => new AccessCodeResponse(p)));
        }

        var skip = page.Value * size.Value;
        var take = size.Value;

        var response = new AccessCodeResponseCollection()
        {
            CurrentPage = page,
            PageSize = size,
            Total = records.Count,
            Items = records.Skip(skip).Take(take).ToList(),
        };

        return response;
    }

    /// <inheritdoc />
    public async Task<AccessCodeResponse> CreateAccessCodeAsync(string eventId, AccessCodeRequest req)
    {
        var @event = await this._managements
                               .GetEntityIfExistsAsync<EventRecord>(
                                   ManagementsTablePartitionKey,
                                   eventId ?? throw new ArgumentNullException(nameof(eventId)))
                               .ConfigureAwait(false)
                           ?? throw new KeyNotFoundException($"Event ID not found: {eventId}");

        var accessCodeId = Guid.NewGuid().ToString();
        var apiKey = Guid.NewGuid().ToString();
        var record = new AccessCodeRecord()
        {
            PartitionKey = eventId,
            RowKey = req.GitHubAlias,
            AccessCodeId = accessCodeId,
            EventId = eventId,
            Name = req.Name,
            Email = req.Email,
            GitHubAlias = req.GitHubAlias,
            ApiKey = apiKey,
            MaxTokens = @event?.Value?.MaxTokens ?? DefaultMaxTokens,
            EventDateStart = @event?.Value?.EventDateStart,
            EventDateEnd = @event?.Value?.EventDateEnd,
            DateCreated = DateTimeOffset.UtcNow,
        };

        await this._accessCodes.UpsertEntityAsync(record).ConfigureAwait(false);
        var result = await this._accessCodes.GetEntityIfExistsAsync<AccessCodeRecord>(eventId, req.GitHubAlias).ConfigureAwait(false);

        return new AccessCodeResponse(result.Value);
    }

    /// <inheritdoc />
    public async Task<AccessCodeResponse> GetAccessCodeByGitHubAliasAsync(string eventId, string gitHubAlias)
    {
        var result = await this._accessCodes.GetEntityIfExistsAsync<AccessCodeRecord>(
                                                eventId ?? throw new ArgumentNullException(nameof(eventId)),
                                                gitHubAlias ?? throw new ArgumentNullException(nameof(gitHubAlias)))
                                            .ConfigureAwait(false)
                           ?? throw new KeyNotFoundException($"Either Event ID: {eventId} or GitHub alias: {gitHubAlias} not found");

        return new AccessCodeResponse(result.Value);
    }
}
