namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for storage account.
/// </summary>
public class StorageAccountSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "StorageAccount";

    /// <summary>
    /// Gets or sets the <see cref="TableStorageSettings"/> instance.
    /// </summary>
    public TableStorageSettings Table { get; set; } = new();
}