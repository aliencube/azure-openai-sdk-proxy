using AzureOpenAIProxy.ApiApp.Configurations;


namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extension entity for the <see cref="StorageSettings"/> class.
/// </summary>
public static class StorageSettingsExtensions
{  
    /// <summary>
    /// Gets the Storage configuration settings by reading appsettings.json.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="StorageSettings"/> instance.</returns>
    public static StorageSettings GetStorageSettings(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()
            ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

        var settings = configuration.GetSection(StorageSettings.Name).Get<StorageSettings>()
            ?? throw new InvalidOperationException($"{nameof(StorageSettings)} could not be retrieved from the configuration.");

        return settings;
    }
}