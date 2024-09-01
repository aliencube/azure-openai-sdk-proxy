using System.Text.Json.Serialization;

/// <summary>
/// This represent the event detail data for response by admin event endpoint.
/// </summary>
public class DeploymentModelDetails
{
    /// <summary>
    /// Gets or sets the deployment id of the model.
    /// </summary>
    [JsonRequired]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the deployment model name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the version of deployment model.
    /// </summary>
    public string? version { get; set; }
}