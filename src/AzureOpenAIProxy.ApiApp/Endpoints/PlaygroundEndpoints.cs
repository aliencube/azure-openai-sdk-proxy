using Azure;

using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the endpoint entity for events that the logged user joined.
/// </summary>
public static class PlaygroundEndpoints
{
    /// <summary>
    /// Adds the event endpoint.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddListEvents(this WebApplication app)
    {
        // ASSUMPTION: User has already logged in
        var builder = app.MapGet(PlaygroundEndpointUrls.Events, async (
            IPlaygroundService service,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminEventEndpoints));
            logger.LogInformation("Received request to fetch events list");

            try
            {
                var eventDetailsList = await service.GetEvents();
                return Results.Ok(eventDetailsList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred while fetching events list");
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .Produces<List<EventDetails>>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("events")
        .WithName("GetEvents")
        .WithOpenApi(operation =>
        {
            operation.Description = "Gets all events' details that the user joined.";
            operation.Summary = "This endpoint gets all events' details that the user joined.";

            return operation;
        });

        return builder;
    }


    /// <summary>
    /// Adds the get deployment models
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddListDeploymentModels(this WebApplication app)
    {
        // Todo: Issue #170 https://github.com/aliencube/azure-openai-sdk-proxy/issues/170
        var builder = app.MapGet(PlaygroundEndpointUrls.DeploymentModels, (
            [FromRoute] string eventId
        ) =>
        {
            return Results.Ok();
        })
        .Produces<List<DeploymentModelDetails>>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces(statusCode: StatusCodes.Status404NotFound)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("events")
        .WithName("GetDeploymentModels")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Gets all deployment models";
            operation.Description = "This endpoint gets all deployment models avaliable";

            return operation;
        });

        return builder;
    }
}