using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Configurations;

namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extensions entity for the OpenAI settings builder.
/// </summary>
public static class OpenAISettingsBuilderExtensions
{
    /// <summary>
    /// Sets the list of <see cref="OpenAIInstanceSettings"/> instances from the app settings.
    /// </summary>
    /// <param name="builder"><see cref="IOpenAISettingsBuilder"/> instance.</param>
    /// <param name="sp"><see cref="IServiceProvider"/> instance.</param>
    /// <returns>Returns <see cref="IOpenAISettingsBuilder"/> instance.</returns>
    public static IOpenAISettingsBuilder WithAppSettings(this IOpenAISettingsBuilder builder, IServiceProvider sp)
    {
        var configuration = sp.GetService<IConfiguration>()
            ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

        var settings = configuration.GetSection(AzureSettings.Name).GetSection(OpenAISettings.Name).Get<OpenAISettings>()
            ?? throw new InvalidOperationException($"{nameof(OpenAISettings)} could not be retrieved from the configuration.");

        builder.SetOpenAIInstances(settings.Instances);

        return builder;
    }
}
