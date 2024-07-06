namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This represents the options entity for <see cref="OpenAIService"/>.
/// </summary>
public class OpenAIServiceOptions
{
    /// <summary>
    /// Gets or sets the OpenAI API endpoint base URL.
    /// </summary>
    public string? Endpoint { get; set; }

    /// <summary>
    /// Gets or sets the OpenAI API path.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the OpenAI API version.
    /// </summary>
    public string ApiVersion { get; set; } = "2024-04-01-preview";

    /// <summary>
    /// Gets or sets the full OpenAI API request URI.
    /// </summary>
    public string? RequestUri { get; set; }

    /// <summary>
    /// Gets or sets the OpenAI API key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the OpenAI API request payload.
    /// </summary>
    public string? Payload { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens to generate.
    /// </summary>
    public int? MaxTokens { get; set; }
}
