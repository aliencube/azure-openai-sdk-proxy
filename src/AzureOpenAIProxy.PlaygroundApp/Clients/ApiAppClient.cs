using Azure;
using Azure.AI.OpenAI;

using AzureOpenAIProxy.PlaygroundApp.Configurations;

namespace AzureOpenAIProxy.PlaygroundApp.Clients;

public interface IApiAppClient
{
    Task<string> GetPingAsync();

    Task<string> GetChatCompletionsAsync(string accessToken, string gitHubId, string deploymentName, string prompt, int? maxTokens = 2048, float? temperature = 0.7f);
}

public class ApiAppClient(OpenAISettings openai, HttpClient http) : IApiAppClient
{
    private readonly OpenAISettings _openai = openai ?? throw new ArgumentNullException(nameof(openai));
    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));

    public async Task<string> GetPingAsync()
    {
        var result = await this._http.GetStringAsync("Ping")
                                     .ConfigureAwait(false);
        return result;
    }

    public async Task<string> GetChatCompletionsAsync(string accessToken, string gitHubId, string deploymentName, string prompt, int? maxTokens = 2048, float? temperature = 0.7f)
    {
        var endpoint = new Uri(this._openai.Endpoint);
        var credential = new AzureKeyCredential($"{accessToken}|{gitHubId}");
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
