using AzureOpenAIProxy.ApiApp.Configurations;

namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extension entity for the <see cref="OpenApiSettings"/> class.
/// </summary>
public static class OpenApiSettingsExtensions
{
    /// <summary>
    /// Gets the OpenApi configuration settings by reading appsettings.json.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> instance.</param>
    /// <returns>Returns <see cref="OpenApiSettings"/> instance.</returns>
    public static OpenApiSettings GetOpenApiSettings(this IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetService<IConfiguration>()
            ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

        var settings = configuration.GetSection(OpenApiSettings.Name).Get<OpenApiSettings>()
            ?? throw new InvalidOperationException($"{nameof(OpenApiSettings)} could not be retrieved from the configuration.");

        return settings;
    }

    /// <summary>
    /// Gets the OpenApi configuration settings by reading appsettings.json.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="OpenApiSettings"/> instance.</returns>
    public static OpenApiSettings GetOpenApiSettings(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        return GetOpenApiSettings(serviceProvider);
    }
}