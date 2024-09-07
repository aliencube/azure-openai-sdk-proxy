using System.Text.Json.Serialization;

/// <summary>
/// This represent the event detail data for response by admin event endpoint.
/// </summary>
public class DeploymentModelDetails
{
    /// <summary>
    /// Gets or sets the deployment model name.
    /// </summary>
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

}