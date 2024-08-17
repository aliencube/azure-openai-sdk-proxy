namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for Key Vault.
/// </summary>
public class KeyVaultSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "KeyVault";

    /// <summary>
    /// Gets or sets the Key Vault URI.
    /// </summary>
    public string? VaultUri { get; set; }

    /// <summary>
    /// Gets or sets the secret name.
    /// </summary>
    public string? SecretName { get; set; }
}