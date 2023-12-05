using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace AzureOpenAIProxy.ApiApp.Filters;

/// <summary>
/// This represents the attribute entity for parameters to be ignored from the OpenAPI document generation.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class OpenApiParameterIgnoreAttribute : Attribute
{
}

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
        return parameterDescription.ModelMetadata is DefaultModelMetadata metadata
            ? metadata.Attributes.ParameterAttributes.Any(attribute => attribute.GetType() == typeof(OpenApiParameterIgnoreAttribute))
            : false;
    }
}