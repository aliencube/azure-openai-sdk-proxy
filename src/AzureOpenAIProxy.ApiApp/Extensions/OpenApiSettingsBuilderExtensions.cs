using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Configurations;

namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extensions entity for the OpenApi settings builder.
/// </summary>
public static class OpenApiSettingsBuilderExtensions
{
    /// <summary>
    /// Sets the Doc version of <see cref="OpenApiSettings"/> instances from the app settings.
    /// </summary>
    /// <param name="builder"><see cref="IOpenApiSettingsBuilder"/> instance.</param>
    /// <param name="sp"><see cref="IServiceProvider"/> instance.</param>
    /// <returns>Returns <see cref="IOpenApiSettingsBuilder"/> instance.</returns>
    public static IOpenApiSettingsBuilder WithDocVersion(this IOpenApiSettingsBuilder builder, IServiceProvider sp)
    {
        var configuration = sp.GetService<IConfiguration>()
            ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");
        var settings = configuration.GetSection(OpenApiSettings.Name).Get<OpenApiSettings>()
            ?? throw new InvalidOperationException($"{nameof(OpenApiSettings)} could not be retrieved from the configuration.");
                
        builder.SetDocVersion(settings.DocVersion!);

        return builder;
    }
}