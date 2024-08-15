using AzureOpenAIProxy.ApiApp.Configurations;

namespace AzureOpenAIProxy.ApiApp.Builders;

/// <summary>
/// This provides interface to the <see cref="OpenAISettingsBuilder"/> class.
/// </summary>
public interface IOpenAISettingsBuilder
{
    /// <summary>
    /// Sets the list of <see cref="OpenAIInstanceSettings"/> instances.
    /// </summary>
    /// <param name="instances">List of <see cref="OpenAIInstanceSettings"/> instances.</param>
    void SetOpenAIInstances(IEnumerable<OpenAIInstanceSettings>? instances);

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
    private IEnumerable<OpenAIInstanceSettings>? _instances;

    /// <inheritdoc />
    public void SetOpenAIInstances(IEnumerable<OpenAIInstanceSettings>? instances)
    {
        this._instances = instances;
    }

    /// <inheritdoc />
    public OpenAISettings Build()
    {
        var settings = new OpenAISettings() { Instances = (this._instances ?? []).ToList() };

        return settings;
    }
}
