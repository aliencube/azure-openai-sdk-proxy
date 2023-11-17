using System;
using System.Threading;
using System.Threading.Tasks;

using Azure;
using Azure.AI.OpenAI;

namespace Aliencube.Azure.AI.OpenAI.Proxy;

/// <summary>
/// This provides interface to the <see cref="OpenAIClientWrapper"/> class.
/// </summary>
public interface IOpenAIClientWrapper
{
    /// <summary> Return textual completions as configured for a given prompt. </summary>
    /// <param name="completionsOptions">
    ///     The options for this completions request.
    /// </param>
    /// <param name="cancellationToken"> The cancellation token to use. </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="completionsOptions"/> or <paramref name="completionsOptions.DeploymentName"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref name="completionsOptions.DeploymentName"/> is an empty string.
    /// </exception>
    Task<Response<Completions>> GetCompletionsAsync(CompletionsOptions completionsOptions, CancellationToken cancellationToken = default);

    /// <summary> Get chat completions for provided chat context messages. </summary>
    /// <param name="chatCompletionsOptions"> The options for this chat completions request. </param>
    /// <param name="cancellationToken"> The cancellation token to use. </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="chatCompletionsOptions"/> or <paramref name="chatCompletionsOptions.DeploymentName"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref name="chatCompletionsOptions.DeploymentName"/> is an empty string.
    /// </exception>
    Task<Response<ChatCompletions>> GetChatCompletionsAsync(ChatCompletionsOptions chatCompletionsOptions, CancellationToken cancellationToken = default);
}
