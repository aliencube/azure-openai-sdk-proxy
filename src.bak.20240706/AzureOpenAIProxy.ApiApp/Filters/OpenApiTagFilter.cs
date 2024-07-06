using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace AzureOpenAIProxy.ApiApp.Filters;

/// <summary>
/// This represents the document filter entity for global tags.
/// </summary>
public class OpenApiTagFilter : IDocumentFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags =
        [
            new OpenApiTag { Name = "weather", Description = "Weather forecast operations" },
            new OpenApiTag { Name = "openai", Description = "Azure OpenAI operations" },
        ];
    }
}
