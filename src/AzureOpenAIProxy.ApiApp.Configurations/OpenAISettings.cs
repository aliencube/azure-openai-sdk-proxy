namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for Azure OpenAI.
/// </summary>

public class OpenAISettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "OpenAI";

    /// <summary>
    /// Gets the <see cref="Random"/> instance.
    /// </summary>
    public Random Random { get; } = new();

    /// <summary>
    /// Gets or sets the list of <see cref="OpenAIInstanceSettings"/> instances.
    /// </summary>
    public List<OpenAIInstanceSettings> Instances { get; set; } = [];
}
