﻿using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Extensions;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="OpenAIService"/> class.
/// </summary>
public interface IOpenAIService
{
    /// <summary>
    /// Invokes the OpenAI API.
    /// </summary>
    /// <param name="path">OpenAI API path.</param>
    /// <param name="apiVersion">OpenAI API version.</param>
    /// <param name="body">Request payload.</param>
    /// <returns>Returns the response payload.</returns>
    Task<string> InvokeAsync(PathString path, string apiVersion, Stream body);
}

/// <summary>
/// This represents the service entity for OpenAI API.
/// </summary>
/// <param name="aoaiSettings"><see cref="AoaiSettings"/> instance.</param>
/// <param name="http"><see cref="HttpClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
public class OpenAIService(AoaiSettings aoaiSettings, HttpClient http, ILogger<OpenAIService> logger) : IOpenAIService
{
    private readonly AoaiSettings _aoaiSettings = aoaiSettings ?? throw new ArgumentNullException(nameof(aoaiSettings));
    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));
    private readonly ILogger<OpenAIService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public async Task<string> InvokeAsync(PathString path, string apiVersion, Stream body)
    {
        var options = await new OpenAIServiceRequestBuilder()
                                .SetOpenAIInstance(this._aoaiSettings)
                                .SetApiPath(path)
                                .SetApiVersion(apiVersion)
                                .SetRequestPayloadAsync(body)
                                .BuildAsync()
                                .ConfigureAwait(false);

        using var response = await this._http
                                       .PostAsync(options)
                                       .ConfigureAwait(false);

        var result = await response.Content
                                   .ReadAsStringAsync()
                                   .ConfigureAwait(false);

        return result;
    }
}
