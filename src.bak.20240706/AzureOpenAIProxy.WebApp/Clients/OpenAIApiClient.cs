using System.ClientModel;

using Azure;
using Azure.AI.OpenAI;

using OpenAI.Chat;

namespace AzureOpenAIProxy.WebApp.Clients;

public interface IOpenAIApiClient
{
    Task<ClientResult<ChatCompletion>> CompleteChatAsync(OpenAIApiClientOptions apiOptions);
}

public class OpenAIApiClient : IOpenAIApiClient
{
    public Task<ClientResult<ChatCompletion>> CompleteChatAsync(OpenAIApiClientOptions apiOptions)
    {
        var endpoint = new Uri(apiOptions.Endpoint!);
        var credential = new AzureKeyCredential(apiOptions.ApiKey!);
        var openai = new AzureOpenAIClient(endpoint, credential);
        var chat = openai.GetChatClient(apiOptions.DeploymentName!);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(apiOptions.SystemPrompt!),
            new UserChatMessage(apiOptions.UserPrompt!)
        };

        var options = new ChatCompletionOptions
        {
            MaxTokens = apiOptions.MaxToken,
            Temperature = apiOptions.Temperature
        };

        var result = chat.CompleteChatAsync(messages, options);

        return result;
    }
}
