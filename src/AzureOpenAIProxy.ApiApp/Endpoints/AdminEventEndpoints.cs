using AzureOpenAIProxy.ApiApp.Models;

using Microsoft.AspNetCore.Mvc;

using Azure.Data.Tables;

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
        //TODO: 테이블 이름을 어딘가에서 상수처럼 관리하기
        const string TableName = "events";

        var builder = app.MapPost(AdminEndpointUrls.AdminEvents, async (
            [FromServices] TableServiceClient tableStorageService,
            [FromBody] AdminEventDetails payload,
            HttpRequest request) =>
        {
            try{
                //TODO: 테이블 서비스/클라이언트는 의존성 주입 받아서 사용하도록 리팩토링
                await tableStorageService.CreateTableIfNotExistsAsync(TableName);
                var tableClient = tableStorageService.GetTableClient(TableName);

                //TODO: PartitonKey: OrganizerName+OrganizerEmail 조합, RowKey: EventId 으로 제안드립니다!
                //TODO: ITableEntity 상속 정리, EventDetails 제네릭 사용하기
                TableEntity? entity = new TableEntity($"{payload.OrganizerName}_{payload.OrganizerEmail}", payload.EventId)
                {
                    ["EventName"] = payload.Title,
                    ["EventDescription"] = payload.Description,
                    ["EventStartDate"] = payload.DateStart,
                    ["EventEndDate"] = payload.DateEnd,
                    ["TimeZone"] = payload.TimeZone,
                    ["IsActive"] = payload.IsActive,
                    ["OrganizerEmail"] = payload.OrganizerEmail,
                    ["CoorganizerName"] = payload.CoorganizerName,
                    ["CoorganizerEmail"] = payload.CoorganizerEmail,
                    ["MaxTokenCap"] = payload.MaxTokenCap,
                    ["DailyRequestCap"] = payload.DailyRequestCap
                };

                await tableClient.AddEntityAsync(entity);
            } catch (Azure.RequestFailedException ex)
            {
                return Results.BadRequest(ex.Message);
            }

            return await Task.FromResult(Results.Ok());
        })
        // TODO: Check both request/response payloads
        .Accepts<AdminEventDetails>(contentType: "application/json")
        .Produces<AdminEventDetails>(statusCode: StatusCodes.Status200OK, contentType: "application/json")
        // TODO: Check both request/response payloads
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