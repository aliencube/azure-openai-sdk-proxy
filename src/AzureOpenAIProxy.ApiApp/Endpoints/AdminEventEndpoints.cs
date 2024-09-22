using Azure;

using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the endpoint entity for event details by admin
/// </summary>
public static class AdminEventEndpoints
{
    /// <summary>
    /// Adds the admin event endpoint
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddNewAdminEvent(this WebApplication app)
    {
        var builder = app.MapPost(AdminEndpointUrls.AdminEvents, async (
            [FromBody] AdminEventDetails payload,
            IAdminEventService service,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminEventEndpoints));
            logger.LogInformation("Received a new event request");

            if (payload is null)
            {
                logger.LogError("No payload found");

                return Results.BadRequest("Payload is null");
            }

            //try
            //{
            //    var result = await service.CreateEvent(payload);

            //    logger.LogInformation("Created a new event");

            //    return Results.Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError(ex, "Failed to create a new event");

            //    return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            //}

            return await Task.FromResult(Results.Ok());
        })
        .Accepts<AdminEventDetails>(contentType: "application/json")
        .Produces<AdminEventDetails>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status400BadRequest)
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("admin")
        .WithName("CreateAdminEvent")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Create admin event";
            operation.Description = "Create admin event";

            return operation;
        });

        return builder;
    }

    /// <summary>
    /// Adds the get event lists by admin endpoint
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddListAdminEvents(this WebApplication app)
    {
        // Todo: Issue #19 https://github.com/aliencube/azure-openai-sdk-proxy/issues/19
        // Need authorization by admin
        var builder = app.MapGet(AdminEndpointUrls.AdminEvents, () =>
        {
            // Todo: Issue #218 https://github.com/aliencube/azure-openai-sdk-proxy/issues/218
            return Results.Ok();
            // Todo: Issue #218
        })
        .Produces<List<AdminEventDetails>>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("admin")
        .WithName("GetAdminEvents")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Gets all events";
            operation.Description = "This endpoint gets all events";

            return operation;
        });

        return builder;
    }

    /// <summary>
    /// Adds the get event details by admin endpoint
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddGetAdminEvent(this WebApplication app)
    {
        // Todo: Issue #19 https://github.com/aliencube/azure-openai-sdk-proxy/issues/19
        // Need authorization by admin
        var builder = app.MapGet(AdminEndpointUrls.AdminEventDetails, async (
            [FromRoute] Guid eventId,
            IAdminEventService service,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminEventEndpoints));
            logger.LogInformation($"Received request to fetch details for event with ID: {eventId}");

            try
            {
                var details = await service.GetEvent(eventId);
                return details is null? Results.NotFound(): Results.Ok(details);
            }
            catch(RequestFailedException ex)
            {
                if(ex.Status == 404)
                {
                    logger.LogError($"Failed to get event details of {eventId}");
                    return Results.NotFound();
                }

                logger.LogError(ex, $"Error occurred while fetching event details of {eventId} with status {ex.Status}");
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Error occurred while fetching event details of {eventId}");
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .Produces<AdminEventDetails>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces(statusCode: StatusCodes.Status404NotFound)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("admin")
        .WithName("GetAdminEvent")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Gets event details from the given event ID";
            operation.Description = "This endpoint gets the event details from the given event ID.";

            return operation;
        });

        return builder;
    }

    /// <summary>
    /// Adds the update event details by admin endpoint
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddUpdateAdminEvent(this WebApplication app)
    {
        // Todo: Issue #19 https://github.com/aliencube/azure-openai-sdk-proxy/issues/19
        // Need authorization by admin
        var builder = app.MapPut(AdminEndpointUrls.AdminEventDetails, (
            [FromRoute] string eventId,
            [FromBody] AdminEventDetails payload) =>
        {
            // Todo: Issue #203 https://github.com/aliencube/azure-openai-sdk-proxy/issues/203
            return Results.Ok();
        })
        .Accepts<AdminEventDetails>(contentType: "application/json")
        .Produces<AdminEventDetails>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status400BadRequest)
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces(statusCode: StatusCodes.Status404NotFound)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("admin")
        .WithName("UpdateAdminEvent")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Updates event details from the given event ID";
            operation.Description = "This endpoint updates the event details from the given event ID.";

            return operation;
        });

        return builder;
    }
}