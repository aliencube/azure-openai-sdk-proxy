﻿using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Controllers;

/// <summary>
/// This represents the controller entity for management.
/// </summary>
/// <param name="auth"><see cref="IAuthService{T}"/> instance.</param>
/// <param name="management"><see cref="IManagementService"/> instance.</param>
/// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance.</param>
[ApiController]
[Route("api/management")]
public class ManagementController(
    [FromKeyedServices("management")] IAuthService<EventRecord> auth,
    IManagementService management,
    ILogger<ManagementController> logger) : ControllerBase
{
    private readonly IAuthService<EventRecord> _auth = auth ?? throw new ArgumentNullException(nameof(auth));
    private readonly IManagementService _management = management ?? throw new ArgumentNullException(nameof(management));
    private readonly ILogger<ManagementController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpGet("events", Name = "GetListOfEvents")]
    public async Task<IActionResult> GetEventsAsync(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromQuery(Name = "page")] int? page = 0,
        [FromQuery(Name = "size")] int? size = 20
        )
    {
        this._logger.LogInformation("Received a request to get all events");

        var record = await this._auth.ValidateAsync(apiKey);
        if (record == null)
        {
            this._logger.LogError("Invalid API key");

            return new UnauthorizedResult();
        }

        try
        {
            var result = await this._management.GetEventsAsync(page, size);

            this._logger.LogInformation("Completed the request to get all event");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to get all event");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

    [HttpPost("events", Name = "CreateEvent")]
    public async Task<IActionResult> CreateEvent(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromBody] EventRequest req)
    {
        this._logger.LogInformation("Received a request to generate an event");

        var record = await this._auth.ValidateAsync(apiKey);
        if (record == null)
        {
            this._logger.LogError("Invalid API key");

            return new UnauthorizedResult();
        }

        if (req == null)
        {
            this._logger.LogError("No request payload");

            return new BadRequestResult();
        }

        try
        {
            var result = await this._management.CreateEventAsync(req);

            this._logger.LogInformation("Completed the request to generate the event");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to generate the event");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

    [HttpGet("events/{eventId}", Name = "GetEventById")]
    public async Task<IActionResult> GetEventByEventIdAsync(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromRoute] string eventId)
    {
        this._logger.LogInformation("Received a request to get an event details by event ID");

        var record = await this._auth.ValidateAsync(apiKey);
        if (record == null)
        {
            this._logger.LogError("Invalid API key");

            return new UnauthorizedResult();
        }

        if (string.IsNullOrWhiteSpace(eventId))
        {
            this._logger.LogError("No event ID");

            return new NotFoundResult();
        }

        try
        {
            var result = await this._management.GetEvenByIdAsync(eventId);

            this._logger.LogInformation("Completed the request to get an event details by event ID");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to get an event details by event ID");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
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
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromRoute] string eventId,
        [FromBody] AccessCodeRequest req)
    {
        this._logger.LogInformation("Received a request to generate an access code");

        var record = await this._auth.ValidateAsync(apiKey);
        if (record == null)
        {
            this._logger.LogError("Invalid API key");

            return new UnauthorizedResult();
        }

        if (string.IsNullOrWhiteSpace(eventId))
        {
            this._logger.LogError("No event ID");

            return new NotFoundResult();
        }

        if (req == null)
        {
            this._logger.LogError("No request payload");

            return new BadRequestResult();
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

    [HttpGet("events/{eventId}/access-codes/{gitHubAlias}", Name = "GetEventAccessCodeByGitHubAlias")]
    public async Task<IActionResult> GetAccessCodesByEventIdAsync(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromRoute] string eventId,
        [FromRoute] string gitHubAlias)
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }
}
