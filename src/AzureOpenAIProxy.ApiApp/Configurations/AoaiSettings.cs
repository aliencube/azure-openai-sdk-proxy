namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for AOAI.
/// </summary>
public class AoaiSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "AOAI";

    /// <summary>
    /// Gets the <see cref="Random"/> instance.
    /// </summary>
    public Random Random { get; } = new();

    /// <summary>
    /// Gets or sets the list of <see cref="OpenAISettings"/> instances.
    /// </summary>
    public List<OpenAISettings> Instances { get; set; } = [];
}
