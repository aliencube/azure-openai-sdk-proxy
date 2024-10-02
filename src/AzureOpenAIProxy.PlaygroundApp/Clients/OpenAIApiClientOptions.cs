namespace AzureOpenAIProxy.PlaygroundApp.Clients;

/// <summary>
/// This represents the options entity for the OpenAI API client.
/// </summary>
public class OpenAIApiClientOptions
{
    /// <summary>
    /// Gets or sets the OpenAI API endpoint.
    /// </summary>
    public string? Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the OpenAI API key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the deployment name.
    /// </summary>
    public string? DeploymentName { get; set; }

    /// <summary>
    /// Gets or sets the max_tokens value.
    /// </summary>
    [Obsolete("Use MaxOutputTokenCount instead.")]
    public int? MaxTokens
    {
        get { return this.MaxOutputTokenCount; }
        set { this.MaxOutputTokenCount = value; }
    }

    /// <summary>
    /// Gets or sets the max_completion_tokens value.
    /// </summary>
    public int? MaxOutputTokenCount { get; set; }

    /// <summary>
    /// Gets or sets the temperature value.
    /// </summary>
    public float? Temperature { get; set; }

    /// <summary>
    /// Gets or sets the system prompt.
    /// </summary>
    public string? SystemPrompt { get; set; }

    /// <summary>
    /// Gets or sets the user prompt.
    /// </summary>
    public string? UserPrompt { get; set; }
}