using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Azure;
using Azure.AI.OpenAI;

namespace Aliencube.Azure.AI.OpenAI.Proxy;

public class OpenAIClientWrapper(HttpClient http, Uri endpoint, AzureKeyCredential keyCredential) : IOpenAIClientWrapper
{
    private const string ApplicationJson = "application/json";
    private const string SubscriptionKey = "Ocp-Apim-Subscription-Key";

    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));
    private readonly Uri _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
    private readonly AzureKeyCredential _keyCredential = keyCredential ?? throw new ArgumentNullException(nameof(keyCredential));

    /// <inheritdoc />
    public async Task<Response<Completions>> GetCompletionsAsync(CompletionsOptions completionsOptions, CancellationToken cancellationToken = default)
    {
        if (completionsOptions is null)
        {
            throw new ArgumentNullException(nameof(completionsOptions));
        }
        if (string.IsNullOrWhiteSpace(completionsOptions.DeploymentName) == true)
        {
            throw new ArgumentException(nameof(completionsOptions.DeploymentName));
        }

        using var content = new StringContent(JsonSerializer.Serialize<CompletionsOptions>(completionsOptions), Encoding.UTF8, ApplicationJson);
        content.Headers.Add(SubscriptionKey, this._keyCredential.Key);

        var requestUri = new Uri(this._endpoint, ApiEndpoints.Completions);

        using var response = await this._http.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
        var payload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<Response<Completions>>(payload);
    }

    /// <inheritdoc />
    public async Task<Response<ChatCompletions>> GetChatCompletionsAsync(ChatCompletionsOptions chatCompletionsOptions, CancellationToken cancellationToken = default)
    {
        if (chatCompletionsOptions is null)
        {
            throw new ArgumentNullException(nameof(chatCompletionsOptions));
        }
        if (string.IsNullOrWhiteSpace(chatCompletionsOptions.DeploymentName) == true)
        {
            throw new ArgumentException(nameof(chatCompletionsOptions.DeploymentName));
        }

        using var content = new StringContent(JsonSerializer.Serialize<ChatCompletionsOptions>(chatCompletionsOptions), Encoding.UTF8, ApplicationJson);
        content.Headers.Add(SubscriptionKey, this._keyCredential.Key);

        var requestUri = new Uri(this._endpoint, ApiEndpoints.ChatCompletions);

        using var response = await this._http.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
        var payload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<Response<ChatCompletions>>(payload);
    }
}
