namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for Table Storage.
/// </summary>
public class StorageAccountSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "StorageAccount";

    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    public string? ConnectionString { get; set; }
}