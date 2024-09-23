﻿using Azure;
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
public class OpenAIApiClient(HttpClient http) : IOpenAIApiClient
{
    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));
    
    /// <inheritdoc />
    public async Task<string> CompleteChatAsync(OpenAIApiClientOptions clientOptions)
    {
        clientOptions.Endpoint = _http.BaseAddress;
        // test
        clientOptions.ApiKey = "abcdef";
        clientOptions.DeploymentName = "model-gpt4o-20240513";
        clientOptions.SystemPrompt = "You are an AI assistant that helps people find information.";
        clientOptions.MaxTokens = 4096;
        clientOptions.Temperature = 0.7f;

        var credential = new AzureKeyCredential(clientOptions.ApiKey!);
        var openai = new AzureOpenAIClient(clientOptions.Endpoint, credential);
        var chat = openai.GetChatClient(clientOptions.DeploymentName);

        var messages = new List<ChatMessage>()
        {
            new SystemChatMessage(clientOptions.SystemPrompt),
            new UserChatMessage(clientOptions.UserPrompt),
        };
        var options = new ChatCompletionOptions
        {
            MaxTokens = clientOptions.MaxTokens,
            Temperature = clientOptions.Temperature,
        };

        var result = await chat.CompleteChatAsync(messages, options).ConfigureAwait(false);
        var response = result.Value.Content.First().Text;

        return response;
    }
}