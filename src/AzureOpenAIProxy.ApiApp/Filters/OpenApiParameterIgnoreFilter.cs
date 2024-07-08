using AzureOpenAIProxy.ApiApp.Attributes;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace AzureOpenAIProxy.ApiApp.Filters;

/// <summary>
/// This represents the filter entity for parameters to be ignored from the OpenAPI document generation.
/// </summary>
/// <remarks>https://stackoverflow.com/questions/69651135/hide-parameter-from-swagger-swashbuckle</remarks>
public class OpenApiParameterIgnoreFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation == null || context == null || context.ApiDescription?.ParameterDescriptions == null)
        {
            return;
        }

        var parametersToHide = context.ApiDescription.ParameterDescriptions
                                      .Where(ParameterHasIgnoreAttribute);

        if (parametersToHide.Any() == false)
        {
            return;
        }

        foreach (var parameterToHide in parametersToHide)
        {
            var parameter = operation.Parameters
                                     .FirstOrDefault(parameter => parameter.Name.Equals(parameterToHide.Name, StringComparison.InvariantCultureIgnoreCase));
            if (parameter != null)
            {
                operation.Parameters.Remove(parameter);
            }
        }
    }

    private static bool ParameterHasIgnoreAttribute(ApiParameterDescription parameterDescription)
    {
        var result = parameterDescription.CustomAttributes()
                                         .Any(attribute => attribute.GetType() == typeof(OpenApiParameterIgnoreAttribute)) == true;

        return result;
    }
}