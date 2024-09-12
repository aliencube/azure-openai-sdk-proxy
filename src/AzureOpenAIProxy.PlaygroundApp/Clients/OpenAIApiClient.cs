using Azure;
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
    /// <returns>Returns the chat completion result.</returns>
    Task<string> CompleteChatAsync();
}

/// <summary>
/// This represents the OpenAI API client entity.
/// </summary>
public class OpenAIApiClient(OpenAIApiClientOptions clientOptions) : IOpenAIApiClient
{
    private readonly OpenAIApiClientOptions _clientOptions = clientOptions ?? throw new ArgumentNullException(nameof(clientOptions));
    
    /// <inheritdoc />
    public async Task<string> CompleteChatAsync()
    {
        var endpoint = new Uri($"{_clientOptions.Endpoint!.TrimEnd('/')}/api");
        var credential = new AzureKeyCredential(_clientOptions.ApiKey!);
        var openai = new AzureOpenAIClient(endpoint, credential);
        var chat = openai.GetChatClient(_clientOptions.DeploymentName);

        var messages = new List<ChatMessage>()
        {
            new SystemChatMessage(_clientOptions.SystemPrompt),
            new UserChatMessage(_clientOptions.UserPrompt),
        };
        var options = new ChatCompletionOptions
        {
            MaxTokens = _clientOptions.MaxTokens,
            Temperature = _clientOptions.Temperature,
        };

        var result = await chat.CompleteChatAsync(messages, options).ConfigureAwait(false);
        var response = result.Value.Content.First().Text;

        return response;
    }
}
