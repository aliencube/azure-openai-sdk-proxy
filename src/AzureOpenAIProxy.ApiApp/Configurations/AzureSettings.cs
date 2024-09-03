namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for Azure.
/// </summary>
public class AzureSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "Azure";

    /// <summary>
    /// Gets or sets the <see cref="OpenAISettings"/> instance.
    /// </summary>
    public OpenAISettings OpenAI { get; set; } = new();

    /// <summary>
    /// Gets or sets the <see cref="KeyVaultSettings"/> instance.
    /// </summary>
    public KeyVaultSettings KeyVault { get; set; } = new();

    /// <summary>
    /// Gets or sets the <see cref="StorageAccountSettings"/> instance.
    /// </summary>
    public StorageAccountSettings StorageAccount { get; set; } = new();
}
