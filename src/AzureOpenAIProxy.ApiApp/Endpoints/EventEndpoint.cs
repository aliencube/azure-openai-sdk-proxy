using System.Text.Json;
using AzureOpenAIProxy.ApiApp.Models;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Endpoints
{
    /// <summary>
    /// Defines an endpoint for events
    /// </summary>
    public static class EventEndpoint
    {
        /// <summary>
        /// Adds EventEndpoint to WebApplication.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static RouteHandlerBuilder AddEventEndpoint(this WebApplication app)
        {
            // TODO: Parameter validation
            var rt = app.MapGet(EndpointUrls.Events,
                (
                    HttpRequest request
                ) =>
                {
                    // These events are mock for now.
                    var sampleEvents = new List<EventListEntity>
                    {
                        new EventListEntity { EventName = "Opening Hackathon",
                            EventId = Guid.NewGuid().ToString(),
                            StartTime = DateTimeOffset.Now,
                            EndTime = DateTimeOffset.Now.AddDays(1),
                            IsActive = true,
                            OrganizerName = "John Doe",
                            OrganizerEmail = "me@example.com",
                            TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Seoul").Id,
                            Registered = 10,
                            Resources = new List<EventListEntity.ResourceEntity>
                            {
                                new EventListEntity.ResourceEntity { ResourceId = "1", ResourceName = "gpt4o" }
                            }
                        }
                    };
                    // TODO: Should add 404 logic if the server failed to retrieve data with TypedResult
                    return TypedResults.Ok(sampleEvents);
                    // return TypedResults.Problem(statusCode: StatusCodes.Status404NotFound);
                })
                // Note: RouteHandlerBuilder does not have ProducesProblem extension method yet, but it will be added on .NET 9
                // Source: https://github.com/dotnet/aspnetcore/issues/56178
                .Produces<EventListEntity>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
                .Produces<ProblemHttpResult>(statusCode: StatusCodes.Status401Unauthorized, contentType: "application/problem+json")
                .Produces<ProblemHttpResult>(statusCode: StatusCodes.Status404NotFound, contentType: "application/problem+json")
                .Produces<ProblemHttpResult>(statusCode: StatusCodes.Status500InternalServerError, contentType: "application/problem+json")
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
