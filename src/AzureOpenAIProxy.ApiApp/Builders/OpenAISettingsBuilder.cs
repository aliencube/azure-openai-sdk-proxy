using AzureOpenAIProxy.ApiApp.Configurations;

namespace AzureOpenAIProxy.ApiApp.Builders;

/// <summary>
/// This provides interface to the <see cref="OpenAISettingsBuilder"/> class.
/// </summary>
public interface IOpenAISettingsBuilder
{
    /// <summary>
    /// Builds the <see cref="OpenAISettings"/> instance.
    /// </summary>
    /// <returns>Returns the <see cref="OpenAISettings"/> instance.</returns>
    OpenAISettings Build();
}

/// <summary>
/// This represents the builder entity for <see cref="OpenAISettings"/> class.
/// </summary>
public class OpenAISettingsBuilder : IOpenAISettingsBuilder
{
    private OpenAISettings? _settings;

    /// <inheritdoc />
    public OpenAISettings Build()
    {
        this._settings ??= new OpenAISettings();

        return this._settings;
    }
}
