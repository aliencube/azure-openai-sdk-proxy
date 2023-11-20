using System.Text.Json;

using Azure;
using Azure.AI.OpenAI;

using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Options;

using Microsoft.AspNetCore.Mvc;

using static Azure.AI.OpenAI.OpenAIClientOptions;

namespace AzureOpenAIProxy.ApiApp.Controllers;

[ApiController]
[Route("openai")]
public class ChatCompletionsController(AoaiSettings aoaiSettings, ILogger<ChatCompletionsController> logger) : ControllerBase
{
    private readonly AoaiSettings _aoaiSettings = aoaiSettings ?? throw new ArgumentNullException(nameof(aoaiSettings));
    private readonly ILogger<ChatCompletionsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpPost("deployments/{deploymentName}/chat/completions", Name = "GetChatCompletions")]
    [Consumes(typeof(ChatCompletionsOptionsWrapper), "application/json")]
    public async Task<IActionResult> GetChatCompletionsAsync(
        [FromRoute] string deploymentName,
        [FromHeader(Name = "api-key")] string apiKey,
        [FromQuery(Name = "api-version")] string apiVersion)
    {
        using var reader = new StreamReader(this.Request.Body);
        var payload = await reader.ReadToEndAsync();
        var req = JsonSerializer.Deserialize<ChatCompletionsOptionsWrapper>(payload, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        var aoai = this._aoaiSettings
                       .Instances
                       .Skip(this._aoaiSettings.Random.Next(this._aoaiSettings.Instances.Count))
                       .First();

        var endpoint = new Uri(aoai.Endpoint);
        var credential = new AzureKeyCredential(aoai.ApiKey);
        var serviceVersion = Enum.TryParse<ServiceVersion>(apiVersion, true, out var parsed) ? parsed : ServiceVersion.V2023_09_01_Preview;
        var client = new OpenAIClient(endpoint, credential, new OpenAIClientOptions(serviceVersion));

        var options = new ChatCompletionsOptions
        {
            DeploymentName = deploymentName,
            MaxTokens = req.MaxTokens,
            Temperature = req.Temperature,
        };
        foreach (var msg in req.Messages)
        {
            options.Messages.Add(new ChatMessage(msg.Role, msg.Content));
        }

        var result = await client.GetChatCompletionsAsync(options);

        return new OkObjectResult(result);
    }

    [HttpPost("deployments/{deploymentName}/extensions/chat/completions", Name = "GetExtensionsChatCompletions")]
    public async Task<IActionResult> GetExtensionsChatCompletionsAsync(
        [FromRoute] string deploymentName,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] ChatCompletionsOptions req)
    {
        req.DeploymentName = deploymentName;

        var aoai = this._aoaiSettings
                       .Instances
                       .Skip(this._aoaiSettings.Random.Next(this._aoaiSettings.Instances.Count))
                       .First();

        var endpoint = new Uri(aoai.Endpoint);
        var credential = new AzureKeyCredential(aoai.ApiKey);
        var serviceVersion = Enum.TryParse<ServiceVersion>(apiVersion, true, out var parsed) ? parsed : ServiceVersion.V2023_09_01_Preview;
        var client = new OpenAIClient(endpoint, credential, new OpenAIClientOptions(serviceVersion));

        var result = await client.GetChatCompletionsAsync(req);

        return new OkObjectResult(result);
    }
}
