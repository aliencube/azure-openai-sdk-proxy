using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Controllers;

/// <summary>
/// This represents the controller entity for management.
/// </summary>
/// <param name="auth"><see cref="IAuthService"/> instance.</param>
/// <param name="management"><see cref="IManagementService"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
[ApiController]
[Route("management")]
public class ManagementController(
    [FromKeyedServices("management")] IAuthService auth,
    IManagementService management,
    ILogger<ManagementController> logger) : ControllerBase
{
    private readonly IAuthService _auth = auth ?? throw new ArgumentNullException(nameof(auth));
    private readonly IManagementService _management = management ?? throw new ArgumentNullException(nameof(management));
    private readonly ILogger<ManagementController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpGet("events", Name = "GetListOfEvents")]
    public async Task<IActionResult> GetEventsAsync([FromHeader(Name = "Authorization")] string apiKey)
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }

    [HttpPost("events", Name = "CreateEvent")]
    public async Task<IActionResult> CreateEvent(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromBody] ManagementRequest req)
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }

    [HttpGet("events/{eventId}", Name = "GetEventById")]
    public async Task<IActionResult> GetEventByEventIdAsync(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromRoute] string eventId)
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }

    [HttpGet("events/{eventId}/access-codes", Name = "GetListOfEventAccessCodes")]
    public async Task<IActionResult> GetAccessCodesByEventIdAsync(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromRoute] string eventId)
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }

    [HttpPost("events/{eventId}/access-codes", Name = "CreateEventAccessCode")]
    public async Task<IActionResult> CreateEvent(
        [FromRoute] string eventId,
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromBody] AccessCodeRequest req)
    {
        this._logger.LogInformation("Received a request to generate an access code");

        var record = await this._auth.ValidateAsync<ManagementRecord>(apiKey);
        if (record == null)
        {
            this._logger.LogError("Invalid API key");

            return new UnauthorizedResult();
        }

        try
        {
            req.EventId = eventId;
            var result = await this._management.CreateAccessCodeAsync(req);

            this._logger.LogInformation("Completed the request to generate the access code");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to generate the access code");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
