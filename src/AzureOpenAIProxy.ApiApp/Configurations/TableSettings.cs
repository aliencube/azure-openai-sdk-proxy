namespace AzureOpenAIProxy.ApiApp.Configurations;

/// <summary>
/// This represents the settings entity for Azure Table Stroage.
/// </summary>
public class TableSettings
{
    /// <summary>
    /// Gets the name of the configuration settings.
    /// </summary>
    public const string Name = "Table";

    /// <summary>
    /// Gets or sets the table name.
    /// </summary>
    public string? TableName { get; set; }
}