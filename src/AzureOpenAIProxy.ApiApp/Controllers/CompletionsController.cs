using Azure;
using Azure.AI.OpenAI;

using AzureOpenAIProxy.ApiApp.Configurations;

using Microsoft.AspNetCore.Mvc;

using static Azure.AI.OpenAI.OpenAIClientOptions;

namespace AzureOpenAIProxy.ApiApp.Controllers;

[ApiController]
[Route("openai")]
public class CompletionsController(AoaiSettings aoaiSettings, ILogger<CompletionsController> logger) : ControllerBase
{
    private readonly AoaiSettings _aoaiSettings = aoaiSettings ?? throw new ArgumentNullException(nameof(aoaiSettings));
    private readonly ILogger<CompletionsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpPost("deployments/{deploymentName}/completions", Name = "GetCompletions")]
    public async Task<IActionResult> GetCompletionsAsync(
        [FromRoute] string deploymentName,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] CompletionsOptions req)
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

        var result = await client.GetCompletionsAsync(req);

        return new OkObjectResult(result);
    }

    [HttpPost("deployments/{deploymentName}/extensions/completions", Name = "GetExtensionsCompletions")]
    public async Task<IActionResult> GetExtensionsCompletionsAsync(
        [FromRoute] string deploymentName,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromBody] CompletionsOptions req)
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

        var result = await client.GetCompletionsAsync(req);

        return new OkObjectResult(result);
    }
}
