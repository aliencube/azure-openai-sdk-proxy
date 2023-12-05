using System.Text.Json;

using Azure.AI.OpenAI;

using AzureOpenAIProxy.ApiApp.Filters;
using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Controllers;

/// <summary>
/// This represents the controller entity for chat completions.
/// </summary>
/// <param name="auth"><see cref="IAuthService{T}"/> instance.</param>
/// <param name="openai"><see cref="IOpenAIService"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
[ApiController]
[Route("api/openai")]
public class CompletionsController(
    [FromKeyedServices("accesscode")] IAuthService<AccessCodeRecord> auth,
    IOpenAIService openai,
    ILogger<CompletionsController> logger) : ControllerBase
{
    private readonly IAuthService<AccessCodeRecord> _auth = auth ?? throw new ArgumentNullException(nameof(auth));
    private readonly IOpenAIService _openai = openai ?? throw new ArgumentNullException(nameof(openai));
    private readonly ILogger<CompletionsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Invokes the completions API.
    /// </summary>
    /// <param name="deploymentName">Deployment name.</param>
    /// <param name="apiKey">API key.</param>
    /// <param name="apiVersion">API version.</param>
    /// <param name="req"><see cref="CompletionsOptions"/> instance as the request payload.</param>
    /// <returns>Returns the chat completions response.</returns>
    [HttpPost("deployments/{deploymentName}/completions", Name = "GetCompletions")]
    public async Task<IActionResult> GetCompletionsAsync(
        [OpenApiParameterIgnore][FromHeader(Name = "api-key")] string apiKey,
        [FromRoute] string deploymentName,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] CompletionsOptions req)
    {
        this._logger.LogInformation("Received a completion request");

        var record = await this._auth.ValidateAsync(apiKey);
        if (record == null)
        {
            this._logger.LogError("Invalid API key");

            return new UnauthorizedObjectResult("Invalid API key");
        }

        this.Request.Body.Position = 0;
        var options = await this._openai.BuildServiceOptionsAsync(this.Request.Path, apiVersion, this.Request.Body);
        if (options.MaxTokens > record.MaxTokens)
        {
            this._logger.LogError($"The maximum number of tokens must be less than or equal to {record.MaxTokens}");

            return new BadRequestObjectResult($"The maximum number of tokens must be less than or equal to {record.MaxTokens}");
        }

        try
        {
            var response = await this._openai.InvokeAsync(options);
            var result = JsonSerializer.Deserialize<object>(response);

            this._logger.LogInformation("Completed the completion request");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to invoke the completion API");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

    /// <summary>
    /// Invokes the extended completions API.
    /// </summary>
    /// <param name="deploymentName">Deployment name.</param>
    /// <param name="apiKey">API key.</param>
    /// <param name="apiVersion">API version.</param>
    /// <param name="req"><see cref="CompletionsOptions"/> instance as the request payload.</param>
    /// <returns>Returns the completions response.</returns>
    [HttpPost("deployments/{deploymentName}/extensions/completions", Name = "GetExtensionsCompletions")]
    public async Task<IActionResult> GetExtensionsCompletionsAsync(
        [OpenApiParameterIgnore][FromHeader(Name = "api-key")] string apiKey,
        [FromRoute] string deploymentName,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] CompletionsOptions req)
    {
        this._logger.LogInformation("Received an extended completion request");

        var record = await this._auth.ValidateAsync(apiKey);
        if (record == null)
        {
            this._logger.LogError("Invalid API key");

            return new UnauthorizedObjectResult("Invalid API key");
        }

        this.Request.Body.Position = 0;
        var options = await this._openai.BuildServiceOptionsAsync(this.Request.Path, apiVersion, this.Request.Body);
        if (options.MaxTokens > record.MaxTokens)
        {
            this._logger.LogError($"The maximum number of tokens must be less than or equal to {record.MaxTokens}");

            return new BadRequestObjectResult($"The maximum number of tokens must be less than or equal to {record.MaxTokens}");
        }

        try
        {
            var response = await this._openai.InvokeAsync(options);
            var result = JsonSerializer.Deserialize<object>(response);

            this._logger.LogInformation("Completed the extended completion request");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to invoke the extended completion API");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
