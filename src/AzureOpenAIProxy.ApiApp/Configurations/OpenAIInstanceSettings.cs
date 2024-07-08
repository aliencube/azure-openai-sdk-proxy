namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for Azure OpenAI instance.
/// </summary>
public class OpenAIInstanceSettings
{
    /// <summary>
    /// Gets or sets the endpoint of the Azure OpenAI API.
    /// </summary>
    public string? Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API key of the Azure OpenAI API.
    /// </summary>
    public string? ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of Azure OpenAI model deployment names.
    /// </summary>
    public List<string>? DeploymentNames { get; set; } = [];
}