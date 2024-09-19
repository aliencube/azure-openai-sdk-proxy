using AzureOpenAIProxy.ApiApp.Models;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the endpoint entity for resource details by admin
/// </summary>
public static class AdminResourceEndpoints
{
    /// <summary>
    /// Adds the admin resource endpoint
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddNewAdminResource(this WebApplication app)
    {
        var builder = app.MapPost(AdminEndpointUrls.AdminResources, async (
            [FromBody] AdminResourceDetails payload,
            IAdminEventService service,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminResourceEndpoints));
            logger.LogInformation("Received a new resource request");

            if (payload is null)
            {
                logger.LogError("No payload found");

                return Results.BadRequest("Payload is null");
            }

            return await Task.FromResult(Results.Ok());
        })
        .Accepts<AdminResourceDetails>(contentType: "application/json")
        .Produces<AdminResourceDetails>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status400BadRequest)
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("admin")
        .WithName("CreateAdminResource")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Create admin resource";
            operation.Description = "Create admin resource";

            return operation;
        });

        return builder;
    }
}