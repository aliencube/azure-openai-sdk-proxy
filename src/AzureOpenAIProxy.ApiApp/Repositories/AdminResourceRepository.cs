using Azure.Data.Tables;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Repositories;

/// <summary>
/// This provides interfaces to the <see cref="AdminResourceRepository"/> class.
/// </summary>
public interface IAdminResourceRepository
{
    /// <summary>
    /// Creates a new record of resource details.
    /// </summary>
    /// <param name="resourceDetails">Resource details instance.</param>
    /// <returns>Returns the resource details instance created.</returns>
    Task<AdminResourceDetails> CreateResource(AdminResourceDetails resourceDetails);
}

/// <summary>
/// This represents the repository entity for the admin resource.
/// </summary>
public class AdminResourceRepository(TableServiceClient tableServiceClient, StorageAccountSettings storageAccountSettings) : IAdminResourceRepository
{
    private readonly TableServiceClient _tableServiceClient = tableServiceClient ?? throw new ArgumentNullException(nameof(tableServiceClient));
    private readonly StorageAccountSettings _storageAccountSettings = storageAccountSettings ?? throw new ArgumentNullException(nameof(storageAccountSettings));

    /// <inheritdoc />
    public async Task<AdminResourceDetails> CreateResource(AdminResourceDetails resourceDetails)
    {
        TableClient tableClient = await GetTableClientAsync();

        await tableClient.AddEntityAsync(resourceDetails).ConfigureAwait(false);

        return resourceDetails;
    }

    private async Task<TableClient> GetTableClientAsync()
    {
        TableClient tableClient = _tableServiceClient.GetTableClient(_storageAccountSettings.TableStorage.TableName);

        await tableClient.CreateIfNotExistsAsync().ConfigureAwait(false);

        return tableClient;
    }
}

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class AdminResourceRepositoryExtensions
{
    /// <summary>
    /// Adds the <see cref="AdminResourceRepository"/> instance to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAdminResourceRepository(this IServiceCollection services)
    {
        services.AddScoped<IAdminResourceRepository, AdminResourceRepository>();

        return services;
    }
}