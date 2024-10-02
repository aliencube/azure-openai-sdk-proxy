using System.ClientModel;

using Azure.AI.OpenAI;

using OpenAI.Chat;

namespace AzureOpenAIProxy.PlaygroundApp.Clients;

/// <summary>
/// This provides interfaces to the <see cref="OpenAIApiClient"/> class.
/// </summary>
public interface IOpenAIApiClient
{
    /// <summary>
    /// Send a chat completion request to the OpenAI API.
    /// </summary>
    /// <param name="clientOptions"><see cref="OpenAIApiClientOptions"/> instance.</param>
    /// <returns>Returns the chat completion result.</returns>
    Task<string> CompleteChatAsync(OpenAIApiClientOptions clientOptions);
}

/// <summary>
/// This represents the OpenAI API client entity.
/// </summary>
public class OpenAIApiClient : IOpenAIApiClient
{
    /// <inheritdoc />
    public async Task<string> CompleteChatAsync(OpenAIApiClientOptions clientOptions)
    {
        var endpoint = new Uri($"{clientOptions.Endpoint!.TrimEnd('/')}/api");
        var credential = new ApiKeyCredential(clientOptions.ApiKey!);
        var openai = new AzureOpenAIClient(endpoint, credential);
        var chat = openai.GetChatClient(clientOptions.DeploymentName);

        var messages = new List<ChatMessage>()
        {
            new SystemChatMessage(clientOptions.SystemPrompt),
            new UserChatMessage(clientOptions.UserPrompt),
        };
        var options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = clientOptions.MaxOutputTokenCount,
            Temperature = clientOptions.Temperature,
        };

        var result = await chat.CompleteChatAsync(messages, options).ConfigureAwait(false);
        var response = result.Value.Content.First().Text;

        return response;
    }
}
