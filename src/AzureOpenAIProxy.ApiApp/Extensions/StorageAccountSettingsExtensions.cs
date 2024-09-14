using AzureOpenAIProxy.ApiApp.Configurations;

namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extension entity for the <see cref="StorageAccountSettings"/> class.
/// </summary>
public static class StorageAccountSettingsExtensions
{  
    /// <summary>
    /// Gets the storage account configuration settings by reading appsettings.json.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="StorageAccountSettings"/> instance.</returns>
    public static IServiceCollection AddStorageAccountSettings(this IServiceCollection services)
    {
        services.AddSingleton<StorageAccountSettings>(sp => {
            var configuration = sp.GetService<IConfiguration>()
                ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

            var settings = configuration.GetSection(StorageAccountSettings.Name).Get<StorageAccountSettings>()
                ?? throw new InvalidOperationException($"{nameof(StorageAccountSettings)} could not be retrieved from the configuration.");

            return settings;
        });

        return services;
    }
}