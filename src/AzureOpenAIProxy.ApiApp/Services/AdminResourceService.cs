using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Repositories;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="AdminResourceService"/> class.
/// </summary>
public interface IAdminResourceService
{
    /// <summary>
    /// Creates a new resource.
    /// </summary>
    /// <param name="resourceDetails">Resource payload.</param>
    /// <returns>Returns the resource payload created.</returns>
    Task<AdminResourceDetails> CreateResource(AdminResourceDetails resourceDetails);
}

/// <summary>
/// This represents the service entity for admin resource.
/// </summary>
public class AdminResourceService : IAdminResourceService
{
    private readonly IAdminResourceRepository _repository;

    public AdminResourceService(IAdminResourceRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task<AdminResourceDetails> CreateResource(AdminResourceDetails resourceDetails)
    {
        resourceDetails.PartitionKey = PartitionKeys.ResourceDetails;
        resourceDetails.RowKey = resourceDetails.ResourceId.ToString();

        var result = await _repository.CreateResource(resourceDetails).ConfigureAwait(false);
        return result;
    }
}

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>.
/// </summary>
public static class AdminResourceServiceExtensions
{
    /// <summary>
    /// Adds the <see cref="AdminResourceService"/> instance to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAdminResourceService(this IServiceCollection services)
    {
        services.AddScoped<IAdminResourceService, AdminResourceService>();
        return services;
    }
}