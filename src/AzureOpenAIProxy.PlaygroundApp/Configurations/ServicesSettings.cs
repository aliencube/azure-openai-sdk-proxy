namespace AzureOpenAIProxy.PlaygroundApp.Configurations;

/// <summary>
/// This represents the app settings entity for services.
/// </summary>
public class ServicesSettings : Dictionary<string, ServiceSettings>
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "services";
}

/// <summary>
/// This represents the service settings entity.
/// </summary>
public class ServiceSettings
{
    /// <summary>
    /// Gets or sets the HTTP endpoints.
    /// </summary>
    public List<string>? Http { get; set; }

    /// <summary>
    /// Gets or sets the HTTPS endpoints.
    /// </summary>
    public List<string>? Https { get; set; }
}

/// <summary>
/// This represents the app settings entity for service names.
/// </summary>
public class ServiceNamesSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "ServiceNames";

    /// <summary>
    /// Gets or sets the backend service name.
    /// </summary>
    public string? Backend { get; set; }
}
