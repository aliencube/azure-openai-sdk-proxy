using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the OpenAI configuration settings to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddOpenAISettings(this IServiceCollection services)
    {
        services.AddSingleton<OpenAISettings>(p =>
        {
            var configuration = p.GetService<IConfiguration>()
                ?? throw new InvalidOperationException("IConfiguration service is not registered.");
            var settings = configuration.GetSection(OpenAISettings.Name).Get<OpenAISettings>()
                ?? throw new InvalidOperationException("OpenAISettings could not be retrieved from the configuration.");

            return settings;
        });

        return services;
    }
}
