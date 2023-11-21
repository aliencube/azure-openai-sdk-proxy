using Azure;
using Azure.AI.OpenAI;

using AzureOpenAIProxy.PlaygroundApp.Configurations;

namespace AzureOpenAIProxy.PlaygroundApp.Clients;

/// <summary>
/// This provides interfaces to the <see cref="ApiAppClient"/> class.
/// </summary>
public interface IApiAppClient
{
    /// <summary>
    /// Gets the chat completions.
    /// </summary>
    /// <param name="accessToken">Access token</param>
    /// <param name="gitHubId">GitHub ID.</param>
    /// <param name="deploymentName">Deployment name.</param>
    /// <param name="prompt">Prompt to send.</param>
    /// <param name="maxTokens">Maximum token number to send.</param>
    /// <param name="temperature">Temperature to send.</param>
    /// <returns>Returns the chat completions result.</returns>
    Task<string> GetChatCompletionsAsync(
        string accessToken,
        string gitHubId,
        string deploymentName,
        string prompt,
        int? maxTokens = 2048,
        float? temperature = 0.7f);
}

/// <summary>
/// This represents the client entity for the API App.
/// </summary>
/// <param name="openai"><see cref="OpenAISettings"/> instance.</param>
/// <param name="http"><see cref="HttpClient"/> instance.</param>
public class ApiAppClient(OpenAISettings openai, HttpClient http) : IApiAppClient
{
    private readonly OpenAISettings _openai = openai ?? throw new ArgumentNullException(nameof(openai));
    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));

    /// <inheritdoc />
    public async Task<string> GetChatCompletionsAsync(
        string accessToken,
        string gitHubId,
        string deploymentName,
        string prompt,
        int? maxTokens = 2048,
        float? temperature = 0.7f)
    {
        var endpoint = new Uri(this._openai.Endpoint);
        var credential = new AzureKeyCredential($"{accessToken}::{gitHubId}");
        var client = new OpenAIClient(endpoint, credential);

        var options = new ChatCompletionsOptions
        {
            DeploymentName = deploymentName,
            MaxTokens = maxTokens,
            Temperature = temperature,
            Messages =
            {
                new ChatMessage(ChatRole.User, prompt)
            }
        };

        var completions = await client.GetChatCompletionsAsync(options);
        var result = completions.Value.Choices[0].Message.Content;

        return result;
    }
}
