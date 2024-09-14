namespace AzureOpenAIProxy.ApiApp;

/// <summary>
/// This represents the keyvault secret names in appsettings.json 
/// </summary>
public static class KeyVaultSecretNames
{
    /// <summary>
    /// Keyvault secret name for OpenAI instance settings
    /// </summary>
    public const string OpenAI = "OpenAI";

    /// <summary>
    /// Keyvault secret name for table storage connection string
    /// </summary>
    public const string Storage = "Storage";

}