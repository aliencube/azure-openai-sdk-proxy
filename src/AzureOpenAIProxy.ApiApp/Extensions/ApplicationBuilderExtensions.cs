﻿namespace AzureOpenAIProxy.ApiApp.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the OpenAPI-enriched weather forecast endpoint.
    /// </summary>
    /// <param name="app"><see cref="WebApplication"/> instance.</param>
    /// <param name="summaries">List of weather forecast summaries.</param>
    /// <returns>Returns <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseSwaggerUI(this WebApplication app, string basePath)
    {
        if (app.Environment.IsDevelopment() == false)
        {
            return app;
        }

        app.UseSwagger(options =>
        {
            //options.RouteTemplate = $"swagger/{Constants.Version}/swagger.json";
            options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                swaggerDoc.Servers =
                [
                    new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" },
            ];
            });
        });
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"{Constants.Version}/swagger.json", Constants.Title);
        });

        return app;
    }
}
