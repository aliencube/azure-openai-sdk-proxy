using AzureOpenAIProxy.ApiApp.Configurations;

namespace AzureOpenAIProxy.ApiApp.Builders;

/// <summary>
/// This provides interface to the <see cref="OpenApiSettingsBuilder"/> class.
/// </summary>
public interface IOpenApiSettingsBuilder
{
    /// <summary>
    /// Sets the OpenApi Doc version.
    /// </summary>
    /// <param name="docVersion">OpenApi Doc version.</param>
    /// <returns>Returns the <see cref="IOpenApiSettingsBuilder"/> instance.</returns>    
    public IOpenApiSettingsBuilder SetDocVersion(string docVersion);

    /// <summary>
    /// Builds the <see cref="OpenApiSettings"/> instance.
    /// </summary>
    /// <returns>Returns the <see cref="OpenApiSettings"/> instance.</returns>
    public OpenApiSettings Build();
}

/// <summary>
/// This represents the builder entity for <see cref="OpenApiSettings"/> class.
/// </summary>
public class OpenApiSettingsBuilder : IOpenApiSettingsBuilder
{
    private string? _docVersion;

    /// <inheritdoc />
    public IOpenApiSettingsBuilder SetDocVersion(string docVersion)
    {
        this._docVersion = docVersion ?? throw new ArgumentNullException(nameof(docVersion));

        return this;
    }
    
    /// <inheritdoc />
    public OpenApiSettings Build() 
    {
        var settings = new OpenApiSettings() 
        { 
            DocVersion = this._docVersion ?? throw new InvalidOperationException("DocVersion is not set.")
        };

        return settings;
    }
}