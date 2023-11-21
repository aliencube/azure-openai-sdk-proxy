using System.Text.Json;

using Azure.AI.OpenAI;

using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Controllers;

/// <summary>
/// This represents the controller entity for chat completions.
/// </summary>
/// <param name="openai"><see cref="IOpenAIService"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
[ApiController]
[Route("openai")]
public class CompletionsController(IOpenAIService openai, ILogger<CompletionsController> logger) : ControllerBase
{
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
        [FromRoute] string deploymentName,
        [FromHeader(Name = "api-key")] string apiKey,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] CompletionsOptions req)
    {
        this.Request.Body.Position = 0;
        var response = await this._openai.InvokeAsync(this.Request.Path, apiVersion, this.Request.Body);
        var result = JsonSerializer.Deserialize<object>(response);

        return new OkObjectResult(result);
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
        [FromRoute] string deploymentName,
        [FromHeader(Name = "api-key")] string apiKey,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] CompletionsOptions req)
    {
        this.Request.Body.Position = 0;
        var response = await this._openai.InvokeAsync(this.Request.Path, apiVersion, this.Request.Body);
        var result = JsonSerializer.Deserialize<object>(response);

        return new OkObjectResult(result);
    }
}
