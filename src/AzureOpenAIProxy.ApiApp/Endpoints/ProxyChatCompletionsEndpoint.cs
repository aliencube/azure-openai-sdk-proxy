using System.Text.Json;

using AzureOpenAIProxy.ApiApp.Attributes;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.AspNetCore.Mvc;

using OpenAI.Chat;

namespace AzureOpenAIProxy.ApiApp.Endpoints;

/// <summary>
/// This represents the endpoint entity for chat completions.
/// </summary>
public static class ProxyChatCompletionsEndpoint
{
    /// <summary>
    /// Adds the chat completion endpoint.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <returns>Returns <see cref="RouteHandlerBuilder"/> instance.</returns>
    public static RouteHandlerBuilder AddChatCompletions(this WebApplication app)
    {
        var builder = app.MapPost(ProxyEndpointUrls.ChatCompletions, async (
            [OpenApiParameterIgnore][FromHeader(Name = "api-key")] string apiKey,
            [FromRoute] string deploymentName,
            [FromQuery(Name = "api-version")] string apiVersion,
            [FromBody] ChatCompletionOptions payload,
            HttpRequest request,
            IOpenAIService openai,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(ProxyChatCompletionsEndpoint));
            logger.LogInformation("Received a chat completion request");

            request.Body.Position = 0;
            var options = await openai.BuildServiceOptionsAsync(request.Path, deploymentName, apiVersion, request.Body);

            try
            {
                var response = await openai.InvokeAsync(options);
                var result = JsonSerializer.Deserialize<object>(response);

                logger.LogInformation("Completed the chat completion request");

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to invoke the chat completion API");

                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        // TODO: Check both request/response payloads
        .Accepts<ChatCompletionOptions>(contentType: "application/json")
        .Produces<ChatCompletion>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        // TODO: Check both request/response payloads
        .Produces(statusCode: StatusCodes.Status401Unauthorized)
        .Produces<string>(statusCode: StatusCodes.Status500InternalServerError, contentType: "text/plain")
        .WithTags("openai")
        .WithName("GetChatCompletions")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get chat completions";
            operation.Description = "Gets the chat completions";

            return operation;
        });

        return builder;
    }
}
