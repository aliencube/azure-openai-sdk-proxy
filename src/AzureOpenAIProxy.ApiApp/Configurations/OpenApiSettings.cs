namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for Open API.
/// </summary>
public class OpenApiSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "OpenApi";

    /// <summary>
    /// Gets or sets the Open API Doc version.
    /// </summary>
    public string? DocVersion { get; set; } = string.Empty;
}