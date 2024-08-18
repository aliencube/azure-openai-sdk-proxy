using AzureOpenAIProxy.ApiApp.Models;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the endpoint entity for get event details by admin
/// </summary>
public static class AdminEventEndpoints
{
    /// <summary>
    /// Adds the get event details by admin endpoint
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddAdminEvents(this WebApplication app)
    {
        // Todo: Issue #19 https://github.com/aliencube/azure-openai-sdk-proxy/issues/19
        // Need authorization by admin
        var builder = app.MapGet(AdminEndpointUrls.AdminEventDetails, (
            [FromRoute] string eventId) =>
        {
            // Todo: Issue #208 https://github.com/aliencube/azure-openai-sdk-proxy/issues/208
            return Results.Ok();
            // Todo: Issue #208
        })
        .Produces<AdminEventDetails>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("admin")
        .WithName("GetAdminEventDetails")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Gets event details from the given event ID";
            operation.Description = "This endpoint gets the event details from the given event ID.";

            return operation;
        });

        return builder;
    }
}
