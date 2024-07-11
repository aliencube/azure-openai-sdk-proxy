using System.Text.Json;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Services;

namespace AzureOpenAIProxy.ApiApp.Builders;

/// <summary>
/// This provides interfaces to the <see cref="OpenAIServiceRequestBuilder"/> class.
/// </summary>
public interface IOpenAIServiceRequestBuilder
{
    /// <summary>
    /// Sets the OpenAI instance settings.
    /// </summary>
    /// <param name="openaiSettings"><see cref="OpenAISettings"/> instance.</param>
    /// <param name="deploymentName">OpenAI deployment name.</param>
    /// <returns>Returns the <see cref="IOpenAIServiceRequestBuilder"/> instance.</returns>
    IOpenAIServiceRequestBuilder SetOpenAIInstance(OpenAISettings openaiSettings, string deploymentName);

    /// <summary>
    /// Sets the OpenAI API path.
    /// </summary>
    /// <param name="path">OpenAI API path.</param>
    /// <returns>Returns the <see cref="IOpenAIServiceRequestBuilder"/> instance.</returns>
    IOpenAIServiceRequestBuilder SetApiPath(PathString path);

    /// <summary>
    /// Sets the OpenAI API version.
    /// </summary>
    /// <param name="apiVersion">OpenAI API version.</param>
    /// <returns>Returns the <see cref="IOpenAIServiceRequestBuilder"/> instance.</returns>
    IOpenAIServiceRequestBuilder SetApiVersion(string apiVersion);

    /// <summary>
    /// Sets the OpenAI API request payload.
    /// </summary>
    /// <param name="stream"><see cref="Stream"/> instance containing the request payload.</param>
    /// <returns>Returns the <see cref="IOpenAIServiceRequestBuilder"/> instance.</returns>
    Task<IOpenAIServiceRequestBuilder> SetRequestPayloadAsync(Stream stream);

    /// <summary>
    /// Builds the <see cref="OpenAIServiceOptions"/> instance.
    /// </summary>
    /// <returns>Returns the <see cref="OpenAIServiceOptions"/> instance.</returns>
    OpenAIServiceOptions Build();
}

/// <summary>
/// This represents the builder entity for <see cref="OpenAIServiceOptions"/>.
/// </summary>
public class OpenAIServiceRequestBuilder : IOpenAIServiceRequestBuilder
{
    private string? _endpoint;
    private string? _apiKey;
    private string? _path;
    private string? _apiVersion;
    private string? _payload;
    private int? _maxTokens;

    /// <inheritdoc />
    public IOpenAIServiceRequestBuilder SetOpenAIInstance(OpenAISettings openaiSettings, string deploymentName)
    {
        var instances = (openaiSettings ?? throw new ArgumentNullException(nameof(openaiSettings)))
                        .Instances
                        .Where(p => p.DeploymentNames!.Contains(deploymentName) == true);
        var openai = instances.Skip(openaiSettings.Random.Next(instances.Count())).FirstOrDefault();

        this._endpoint = openai?.Endpoint;
        this._apiKey = openai?.ApiKey;

        return this;
    }

    /// <inheritdoc />
    public IOpenAIServiceRequestBuilder SetApiPath(PathString path)
    {
        this._path = path.Value;

        return this;
    }

    /// <inheritdoc />
    public IOpenAIServiceRequestBuilder SetApiVersion(string apiVersion)
    {
        this._apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));

        return this;
    }

    /// <inheritdoc />
    public async Task<IOpenAIServiceRequestBuilder> SetRequestPayloadAsync(Stream stream)
    {
        using var jd = await JsonDocument.ParseAsync(stream ?? throw new ArgumentNullException(nameof(stream)));
        this._payload = jd.RootElement.GetRawText();
        this._maxTokens = jd.RootElement.GetProperty("max_tokens").GetInt32();

        return this;
    }

    /// <inheritdoc />
    public OpenAIServiceOptions Build()
    {
        var options = new OpenAIServiceOptions
        {
            Endpoint = this._endpoint ?? throw new InvalidOperationException("Endpoint is not set."),
            Path = this._path ?? throw new InvalidOperationException("Path is not set."),
            ApiVersion = this._apiVersion ?? throw new InvalidOperationException("ApiVersion is not set."),
            RequestUri = $"{this._endpoint?.TrimEnd('/')}/{this._path?.Trim('/')}?api-version={this._apiVersion}",
            ApiKey = this._apiKey,
            Payload = this._payload,
            MaxTokens = this._maxTokens,
        };

        return options.RequestUri.Contains("null")
            ? throw new InvalidOperationException("RequestUri contains null values, make sure Endpoint, Path and ApiVersion are set before building.")
            : options;
    }
}
