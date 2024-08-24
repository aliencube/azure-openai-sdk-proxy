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

    /// <summary>
    /// Adds the get event lists by admin endpoint
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddAdminEventList(this WebApplication app)
    {
        // Todo: Issue #19 https://github.com/aliencube/azure-openai-sdk-proxy/issues/19
        // Need authorization by admin
        //TODO: [tae0y] 파라미터 없이 이벤트를 전체 조회하는게 맞는가?
        var builder = app.MapGet(AdminEndpointUrls.AdminEventList, () =>
        {
            // Todo: Issue #218 https://github.com/aliencube/azure-openai-sdk-proxy/issues/218
            return Results.Ok();
            // Todo: Issue #218
        })
        //TODO: [tae0y] 이벤트 목록 조회할때는 불필요한 필드가 많은데 어떻게 처리할까?
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
}