namespace AzureOpenAIProxy.ApiApp.Attributes;

/// <summary>
/// This represents the attribute entity for parameters to be ignored from the OpenAPI document generation.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class OpenApiParameterIgnoreAttribute : Attribute
{
}
