using AzureOpenAIProxy.ApiApp.Models;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Controllers;

/// <summary>
/// This represents the controller entity for management.
/// </summary>
public partial class ManagementController
{
    /// <summary>
    /// Gets the list of access codes by event ID.
    /// </summary>
    /// <param name="apiKey">API key.</param>
    /// <param name="eventId">Event ID.</param>
    /// <param name="page">Page number.</param>
    /// <param name="size">Page size.</param>
    /// <returns>Returns the <see cref="AccessCodeResponseCollection"/> instance.</returns>
    [HttpGet("events/{eventId}/access-codes", Name = "GetListOfEventAccessCodes")]
    public async Task<IActionResult> GetEventAccessCodesByEventIdAsync(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromRoute] string eventId,
        [FromQuery(Name = "page")] int? page = 0,
        [FromQuery(Name = "size")] int? size = 20)
    {
        this._logger.LogInformation("Received a request to get the list of access codes for the given event");

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
            var result = await this._management.GetAccessCodesAsync(eventId, page, size);

            this._logger.LogInformation("Completed the request to get the list of access codes for the given event");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to get the list of access codes for the given event");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

    /// <summary>
    /// Creates the access code.
    /// </summary>
    /// <param name="apiKey">API key.</param>
    /// <param name="eventId">Event ID.</param>
    /// <param name="req"><see cref="AccessCodeRequest"/> instance.</param>
    /// <returns>Returns the <see cref="AccessCodeResponse"/> instance.</returns>
    [HttpPost("events/{eventId}/access-codes", Name = "CreateEventAccessCode")]
    public async Task<IActionResult> CreateEventAccessCodeAsync(
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
            var result = await this._management.CreateAccessCodeAsync(eventId, req);

            this._logger.LogInformation("Completed the request to generate the access code");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to generate the access code");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

    /// <summary>
    /// Gets the access code by GitHub alias.
    /// </summary>
    /// <param name="apiKey">API key.</param>
    /// <param name="eventId">Event ID.</param>
    /// <param name="gitHubAlias">GitHub alias.</param>
    /// <returns>Returns the <see cref="AccessCodeResponse"/> instance.</returns>
    [HttpGet("events/{eventId}/access-codes/{gitHubAlias}", Name = "GetEventAccessCodeByGitHubAlias")]
    public async Task<IActionResult> GetEventAccessCodeByGitHubAliasAsync(
        [FromHeader(Name = "Authorization")] string apiKey,
        [FromRoute] string eventId,
        [FromRoute] string gitHubAlias)
    {
        this._logger.LogInformation("Received a request to get the access code belongs to the given GitHub alias");

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

        if (string.IsNullOrWhiteSpace(gitHubAlias))
        {
            this._logger.LogError("No GitHub alias");

            return new NotFoundResult();
        }

        try
        {
            var result = await this._management.GetAccessCodeByGitHubAliasAsync(eventId, gitHubAlias);

            this._logger.LogInformation("Completed the request to get the access code belongs to the given GitHub alias");

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Failed to get the access code belongs to the given GitHub alias");

            return new ObjectResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
