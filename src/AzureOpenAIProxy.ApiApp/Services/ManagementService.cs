﻿using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="ManagementService"/> class.
/// </summary>
public interface IManagementService
{
    /// <summary>
    /// Gets the list of events.
    /// </summary>
    /// <param name="page">Page number.</param>
    /// <param name="size">Page size.</param>
    /// <returns>Returns the <see cref="EventResponseCollection"/> instance.</returns>
    Task<EventResponseCollection> GetEventsAsync(int? page = 0, int? size = 20);

    /// <summary>
    /// Creates the event.
    /// </summary>
    /// <param name="req"><see cref="EventRequest"/> instance.</param>
    /// <returns>Returns the <see cref="EventResponse"/> instance.</returns>
    Task<EventResponse> CreateEventAsync(EventRequest req);

    /// <summary>
    /// Get the event by ID.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <returns>Returns the <see cref="EventResponse"/> instance.</returns>
    Task<EventResponse> GetEvenByIdAsync(string eventId);

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
    public async Task<EventResponseCollection> GetEventsAsync(int? page = 0, int? size = 20)
    {
        var results = this._managements.QueryAsync<EventRecord>(p => p.PartitionKey
                                                                      .Equals(ManagementsTablePartitionKey, StringComparison.InvariantCultureIgnoreCase));
        var records = new List<EventResponse>();
        await foreach (var result in results.AsPages())
        {
            records.AddRange(result.Values.Select(p => new EventResponse(p)));
        }

        var skip = page.Value * size.Value;
        var take = size.Value;

        var response = new EventResponseCollection()
        {
            CurrentPage = page,
            PageSize = size,
            Total = records.Count,
            Items = records.Skip(skip).Take(take).ToList(),
        };

        return response;
    }

    /// <inheritdoc />
    public async Task<EventResponse> CreateEventAsync(EventRequest req)
    {
        var eventId = Guid.NewGuid().ToString();
        var apiKey = Guid.NewGuid().ToString();
        var record = new EventRecord()
        {
            PartitionKey = ManagementsTablePartitionKey,
            RowKey = eventId,
            EventId = eventId,
            EventName = req.EventName,
            EventDescription = req.EventDescription,
            EventOrganiser = req.EventOrganiser,
            EventOrganiserEmail = req.EventOrganiserEmail,
            EventDateStart = req.EventDateStart.Value.ToUniversalTime(),
            EventDateEnd = req.EventDateEnd.Value.ToUniversalTime(),
            ApiKey = apiKey,
        };

        await this._managements.UpsertEntityAsync(record).ConfigureAwait(false);
        var result = await this._managements.GetEntityIfExistsAsync<EventRecord>(ManagementsTablePartitionKey, eventId).ConfigureAwait(false);

        return new EventResponse(result.Value);
    }

    /// <inheritdoc />
    public async Task<EventResponse> GetEvenByIdAsync(string eventId)
    {
        var result = await this._managements.GetEntityIfExistsAsync<EventRecord>(
                                                ManagementsTablePartitionKey,
                                                eventId ?? throw new ArgumentNullException(nameof(eventId)))
                                            .ConfigureAwait(false);

        return new EventResponse(result.Value);
    }

    /// <inheritdoc />
    public async Task<AccessCodeResponse> CreateAccessCodeAsync(AccessCodeRequest req)
    {
        var @event = await this._managements
                               .GetEntityIfExistsAsync<EventRecord>(ManagementsTablePartitionKey, req.EventId)
                               .ConfigureAwait(false)
                           ?? throw new KeyNotFoundException($"Event ID not found: {req.EventId}");

        var apiKey = Guid.NewGuid().ToString();
        var record = new AccessCodeRecord()
        {
            PartitionKey = req.EventId,
            RowKey = req.GitHubAlias,
            EventId = req.EventId,
            Name = req.Name,
            Email = req.Email,
            GitHubAlias = req.GitHubAlias,
            ApiKey = apiKey,
            EventDateStart = @event?.Value?.EventDateStart,
            EventDateEnd = @event?.Value?.EventDateEnd,
            DateCreated = DateTimeOffset.UtcNow,
        };

        await this._accessCodes.UpsertEntityAsync(record).ConfigureAwait(false);
        var result = await this._accessCodes.GetEntityIfExistsAsync<AccessCodeRecord>(req.EventId, apiKey).ConfigureAwait(false);

        return new AccessCodeResponse(result.Value);
    }
}
