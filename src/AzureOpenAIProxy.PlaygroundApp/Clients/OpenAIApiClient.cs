using Azure;
using Azure.AI.OpenAI;

using OpenAI.Chat;

namespace AzureOpenAIProxy.PlaygroundApp.Clients;

public interface IOpenAIApiClient
{
    Task<string> CompleteChatAsync(OpenAIApiClientOptions clientOptions);
}

public class OpenAIApiClient : IOpenAIApiClient
{
    public async Task<string> CompleteChatAsync(OpenAIApiClientOptions clientOptions)
    {
        var endpoint = new Uri(clientOptions.Endpoint!);
        var credential = new AzureKeyCredential(clientOptions.ApiKey!);
        var openai = new AzureOpenAIClient(endpoint, credential);
        var chat = openai.GetChatClient(clientOptions.DeploymentName!);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(clientOptions.SystemPrompt!),
            new UserChatMessage(clientOptions.UserPrompt!)
        };

        var options = new ChatCompletionOptions
        {
            MaxTokens = clientOptions.MaxToken,
            Temperature = clientOptions.Temperature
        };

        var result = await chat.CompleteChatAsync(messages, options).ConfigureAwait(false);
        var response = result.Value.Content[0].Text;

        return response;
    }
}

public class OpenAIApiClientOptions
{
    public string? Endpoint { get; set; }
    public string? ApiKey { get; set; }
    public string? DeploymentName { get; set; }
    public string? SystemPrompt { get; set; }
    public string? UserPrompt { get; set; }
    public int? MaxToken { get; set; }
    public float? Temperature { get; set; }
}