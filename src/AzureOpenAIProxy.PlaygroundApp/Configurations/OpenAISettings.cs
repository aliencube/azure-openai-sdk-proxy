namespace AzureOpenAIProxy.PlaygroundApp.Configurations;

/// <summary>
/// This represents the settings entity for OpenAI.
/// </summary>
public class OpenAISettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "OpenAI";

    /// <summary>
    /// Gets or sets the endpoint of the OpenAI API.
    /// </summary>
    public string? Endpoint { get; set; } = string.Empty;
}
