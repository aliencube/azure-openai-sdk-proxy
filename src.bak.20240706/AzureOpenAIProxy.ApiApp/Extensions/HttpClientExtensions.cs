using System.Text;

using AzureOpenAIProxy.ApiApp.Services;

namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extension entity for <see cref="HttpClient"/>.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Invoke the POST request to the OpenAI API.
    /// </summary>
    /// <param name="http"><see cref="HttpClient"/> instance.</param>
    /// <param name="options"><see cref="OpenAIServiceOptions"/> instance.</param>
    /// <returns><see cref="HttpResponseMessage"/> instance.</returns>
    public static async Task<HttpResponseMessage> PostAsync(this HttpClient http, OpenAIServiceOptions options)
    {
        if (options.Payload == null)
        {
            throw new ArgumentNullException(nameof(options), "Payload cannot be null.");
        }

        http.DefaultRequestHeaders.Add("api-key", options.ApiKey);

        var content = new StringContent(options.Payload, Encoding.UTF8, "application/json");
        var response = await http.PostAsync(options.RequestUri, content)
                                 .ConfigureAwait(false);

        return response;
    }
}