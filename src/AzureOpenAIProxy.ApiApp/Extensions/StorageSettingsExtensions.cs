using AzureOpenAIProxy.ApiApp.Configurations;


namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extension entity for the <see cref="StorageAccountSettings"/> class.
/// </summary>
public static class StorageSettingsExtensions
{  
    /// <summary>
    /// Gets the Storage configuration settings by reading appsettings.json.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="StorageAccountSettings"/> instance.</returns>
    public static StorageAccountSettings GetStorageSettings(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()
            ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

        var settings = configuration.GetSection(StorageAccountSettings.Name).Get<StorageAccountSettings>()
            ?? throw new InvalidOperationException($"{nameof(StorageAccountSettings)} could not be retrieved from the configuration.");

        return settings;
    }
}