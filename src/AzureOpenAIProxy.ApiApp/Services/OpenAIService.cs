using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Extensions;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="OpenAIService"/> class.
/// </summary>
public interface IOpenAIService
{
    /// <summary>
    /// Builds the <see cref="OpenAIServiceOptions"/> instance.
    /// </summary>
    /// <param name="path">OpenAI API path.</param>
    /// <param name="deploymentName">OpenAI deployment name.</param>
    /// <param name="apiVersion">OpenAI API version.</param>
    /// <param name="body">Request payload.</param>
    /// <returns>Returns the <see cref="OpenAIServiceOptions"/> instance.</returns>
    Task<OpenAIServiceOptions> BuildServiceOptionsAsync(PathString path, string deploymentName, string apiVersion, Stream body);

    /// <summary>
    /// Invokes the OpenAI API.
    /// </summary>
    /// <param name="options"><see cref="OpenAIServiceOptions"/> instance.</param>
    /// <returns>Returns the response payload.</returns>
    Task<string> InvokeAsync(OpenAIServiceOptions options);
}

/// <summary>
/// This represents the service entity for OpenAI API.
/// </summary>
/// <param name="openaiSettings"><see cref="OpenAISettings"/> instance.</param>
/// <param name="http"><see cref="HttpClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
public class OpenAIService(OpenAISettings openaiSettings, HttpClient http, ILogger<OpenAIService> logger) : IOpenAIService
{
    private readonly OpenAISettings _openaiSettings = openaiSettings ?? throw new ArgumentNullException(nameof(openaiSettings));
    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));
    private readonly ILogger<OpenAIService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<OpenAIServiceOptions> BuildServiceOptionsAsync(PathString path, string deploymentName, string apiVersion, Stream body)
    {
        var options = await new OpenAIServiceRequestBuilder()
                                .SetOpenAIInstance(this._openaiSettings, deploymentName)
                                .SetApiPath(path)
                                .SetApiVersion(apiVersion)
                                .SetRequestPayloadAsync(body)
                                .BuildAsync()
                                .ConfigureAwait(false);
        return options;
    }

    /// <inheritdoc />
    public async Task<string> InvokeAsync(OpenAIServiceOptions options)
    {
        try
        {
            using var response = await this._http
                                           .PostAsync(options)
                                           .ConfigureAwait(false);

            var result = await response.Content
                                       .ReadAsStringAsync()
                                       .ConfigureAwait(false);

            return result;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
