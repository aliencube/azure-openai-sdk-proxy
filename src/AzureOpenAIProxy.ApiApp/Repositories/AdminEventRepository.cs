using AzureOpenAIProxy.ApiApp.Models;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace AzureOpenAIProxy.ApiApp.Repositories;

/// <summary>
/// This provides interfaces to the <see cref="AdminEventRepository"/> class.
/// </summary>
public interface IAdminEventRepository
{
    /// <summary>
    /// Creates a new record of event details.
    /// </summary>
    /// <param name="eventDetails">Event details instance.</param>
    /// <returns>Returns the event details instance created.</returns>
    Task<AdminEventDetails> CreateEvent(AdminEventDetails eventDetails);

    /// <summary>
    /// Gets the list of events.
    /// </summary>
    /// <returns>Returns the list of events.</returns>
    Task<List<AdminEventDetails>> GetEvents();

    /// <summary>
    /// Gets the event details.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <returns>Returns the event details record.</returns>
    Task<AdminEventDetails> GetEvent(Guid eventId);

    /// <summary>
    /// Updates the event details.
    /// </summary>
    /// <param name="eventId">Event ID.</param>
    /// <param name="eventDetails">Event details instance.</param>
    /// <returns>Returns the updated record of the event details.</returns>
    Task<AdminEventDetails> UpdateEvent(Guid eventId, AdminEventDetails eventDetails);
}

/// <summary>
/// This represents the repository entity for the admin event.
/// </summary>
public class AdminEventRepository : IAdminEventRepository
{
    private readonly string TableName = "events";

    private readonly TableServiceClient _tableServiceClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminEventRepository"/> class.
    /// </summary>
    public AdminEventRepository()
    {
        // TODO: [tae0y] 빌드 실패 방지용 임시코드
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminEventRepository"/> class.
    /// </summary>
    public AdminEventRepository(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient ?? throw new ArgumentNullException(nameof(tableServiceClient));
    }

    /// <inheritdoc />
    public async Task<AdminEventDetails> CreateEvent(AdminEventDetails eventDetails)
    {
        var tableServiceClient = _tableServiceClient.GetTableClient(TableName);

        // 데이터 저장
        var createResponse = await tableServiceClient.AddEntityAsync(eventDetails).ConfigureAwait(false);

        // 저장한 데이터 재조회
        // TODO: [tae0y] Azure.Tables REST API는 저장한 Entity를 반환하는 옵션이 있으나, tableServiceClient는 없으므로 추가 작업 필요
        var getResponse = await tableServiceClient.GetEntityAsync<AdminEventDetails>(
            eventDetails.PartitionKey,
            eventDetails.RowKey
        ).ConfigureAwait(false);

        // 조회한 엔티티 반환
        return getResponse.Value;
    }

    /// <inheritdoc />
    public async Task<List<AdminEventDetails>> GetEvents()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<AdminEventDetails> GetEvent(Guid eventId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<AdminEventDetails> UpdateEvent(Guid eventId, AdminEventDetails eventDetails)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class AdminEventRepositoryExtensions
{
    /// <summary>
    /// Adds the <see cref="AdminEventRepository"/> instance to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAdminEventRepository(this IServiceCollection services)
    {
        services.AddScoped<IAdminEventRepository, AdminEventRepository>();

        return services;
    }
}
