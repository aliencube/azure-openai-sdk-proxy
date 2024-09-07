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
        var builder = app.MapGet(PlaygroundEndpointUrls.Events, () =>
        {
            // TODO: Issue #179 https://github.com/aliencube/azure-openai-sdk-proxy/issues/179
            return Results.Ok();
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
}