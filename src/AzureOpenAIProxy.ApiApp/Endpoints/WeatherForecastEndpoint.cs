using AzureOpenAIProxy.ApiApp.Models;

namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the endpoint entity for weather forecast.
/// </summary>
public static class WeatherForecastEndpoint
{
    private static readonly string[] summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

    /// <summary>
    /// Adds the weather forecast endpoint.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddWeatherForecast(this WebApplication app)
    {
        var builder = app.MapGet(EndpointUrls.WeatherForecast, () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();

            return Results.Ok(forecast);
        })
        .Produces<WeatherForecast>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("weather")
        .WithName("GetWeatherForecast")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get weather forecast";
            operation.Description = "Gets the weather forecast";

            return operation;
        });

        return builder;
    }
}
