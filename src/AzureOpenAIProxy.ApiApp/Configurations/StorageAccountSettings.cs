namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for Storage.
/// </summary>
public class StorageAccountSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "StorageAccount";

    /// <summary>
    /// Gets or sets the <see cref="TableSettings"/> instance.
    /// </summary>
    public TableSettings Table { get; set; } = new();
}