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
public class ChatCompletionsController(IOpenAIService openai, ILogger<ChatCompletionsController> logger) : ControllerBase
{
    private readonly IOpenAIService _openai = openai ?? throw new ArgumentNullException(nameof(openai));
    private readonly ILogger<ChatCompletionsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Invokes the chat completions API.
    /// </summary>
    /// <param name="deploymentName">Deployment name.</param>
    /// <param name="apiKey">API key.</param>
    /// <param name="apiVersion">API version.</param>
    /// <param name="req"><see cref="ChatCompletionsOptions"/> instance as the request payload.</param>
    /// <returns>Returns the chat completions response.</returns>
    [HttpPost("deployments/{deploymentName}/chat/completions", Name = "GetChatCompletions")]
    public async Task<IActionResult> GetChatCompletionsAsync(
        [FromRoute] string deploymentName,
        [FromHeader(Name = "api-key")] string apiKey,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] ChatCompletionsOptions req)
    {
        this.Request.Body.Position = 0;
        var response = await this._openai.InvokeAsync(this.Request.Path, apiVersion, this.Request.Body);
        var result = JsonSerializer.Deserialize<object>(response);

        return new OkObjectResult(result);
    }

    /// <summary>
    /// Invokes the extended chat completions API.
    /// </summary>
    /// <param name="deploymentName">Deployment name.</param>
    /// <param name="apiKey">API key.</param>
    /// <param name="apiVersion">API version.</param>
    /// <param name="req"><see cref="ChatCompletionsOptions"/> instance as the request payload.</param>
    /// <returns>Returns the chat completions response.</returns>
    [HttpPost("deployments/{deploymentName}/extensions/chat/completions", Name = "GetExtensionsChatCompletions")]
    public async Task<IActionResult> GetExtensionsChatCompletionsAsync(
        [FromRoute] string deploymentName,
        [FromHeader(Name = "api-key")] string apiKey,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] ChatCompletionsOptions req)
    {
        this.Request.Body.Position = 0;
        var response = await this._openai.InvokeAsync(this.Request.Path, apiVersion, this.Request.Body);
        var result = JsonSerializer.Deserialize<object>(response);

        return new OkObjectResult(result);
    }
}
