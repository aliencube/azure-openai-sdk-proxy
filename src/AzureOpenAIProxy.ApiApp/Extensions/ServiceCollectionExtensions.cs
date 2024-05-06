﻿using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Filters;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.OpenApi.Models;

namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the OpenAI service to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddOpenAIService(this IServiceCollection services)
    {
        services.AddOpenAISettings();
        services.AddHttpClient<IOpenAIService, OpenAIService>();

        return services;
    }

    /// <summary>
    /// Adds the OpenAPI service to the services collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddOpenApiService(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var info = new OpenApiInfo()
            {
                Version = Constants.Version,
                Title = Constants.Title,
                Description = "Providing a proxy service to Azure OpenAI API",
                Contact = new OpenApiContact()
                {
                    Name = "Azure OpenAI Proxy API",
                    Email = "aoai-proxy@contoso.com",
                    Url = new Uri("https://aka.ms/aoai-proxy.net")
                },
            };
            options.SwaggerDoc(Constants.Version, info);

            options.AddSecurityDefinition(
                "apiKey",
                new OpenApiSecurityScheme()
                {
                    Name = "api-key",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "API key needed to access the endpoints.",
                    In = ParameterLocation.Header,
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "apiKey" }
                    },
                    Array.Empty<string>()
                }
            });

            options.DocumentFilter<OpenApiTagFilter>();
            options.OperationFilter<OpenApiParameterIgnoreFilter>();
        });

        return services;
    }
}
