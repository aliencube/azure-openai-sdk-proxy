using System.ClientModel;

using Azure.AI.OpenAI;

using AzureOpenAIProxy.PlaygroundApp.Configurations;

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
public class OpenAIApiClient(ServiceNamesSettings names, ServicesSettings settings) : IOpenAIApiClient
{
    private readonly ServiceNamesSettings _names = names ?? throw new ArgumentNullException(nameof(names));
    private readonly ServicesSettings _settings = settings ?? throw new ArgumentNullException(nameof(settings));

    /// <inheritdoc />
    public async Task<string> CompleteChatAsync(OpenAIApiClientOptions clientOptions)
    {
        var service = this._settings[this._names.Backend!];
        var endpoint = service.Https!.FirstOrDefault() ?? service.Http!.First();

        clientOptions.Endpoint = new Uri($"{endpoint!.TrimEnd('/')}/api");

        var credential = new ApiKeyCredential(clientOptions.ApiKey!);
        var openai = new AzureOpenAIClient(clientOptions.Endpoint, credential);
        var chat = openai.GetChatClient(clientOptions.DeploymentName);

        var messages = new List<ChatMessage>()
        {
            new SystemChatMessage(clientOptions.SystemPrompt), new UserChatMessage(clientOptions.UserPrompt),
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