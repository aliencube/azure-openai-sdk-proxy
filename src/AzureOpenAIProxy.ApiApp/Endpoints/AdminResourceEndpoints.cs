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
            IAdminResourceService service,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(AdminResourceEndpoints));
            logger.LogInformation("Received a new resource request");

            if (payload is null)
            {
                logger.LogError("No payload found");

                return Results.BadRequest("Payload is null");
            }

            try
            {
                var result = await service.CreateResource(payload);

                logger.LogInformation("Created a new resource");

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create a new resource");

                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
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