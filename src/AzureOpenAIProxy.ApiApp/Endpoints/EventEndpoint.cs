using System.Text.Json;
using AzureOpenAIProxy.ApiApp.Models;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Endpoints
{
    /// <summary>
    /// This represents the endpoint entity for events.
    /// </summary>
    public static class EventEndpoint
    {
        /// <summary>
        /// Adds the event endpoint.
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/> instance.</param>
        /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
        public static RouteHandlerBuilder AddEventEndpoint(this WebApplication app)
        {
            var rt = app.MapGet(EndpointUrls.Events, () =>
                {
                // TODO: Implement logic of endpoints
                // Need authorization
                return Results.Ok();
                })
                .Produces<EventListEntity>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
            .Produces(statusCode: StatusCodes.Status401Unauthorized)
            .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
                .WithTags("events")
                .WithName("GetEvents")
                .WithOpenApi(operations =>
                {
                    operations.Description = "Show all events in Azure OpenAI Proxy services.";
                    operations.Summary = "Get all events";
                    return operations;
                });

            return rt;
        }
    }
}
