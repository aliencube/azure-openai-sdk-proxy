using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary> Information about filtered content severity level and if it has been filtered or not. </summary>
public class ContentFilterResultsModel
{
    /// <summary> Ratings for the intensity and risk level of filtered content. </summary>
    [JsonPropertyName("severity")]
    public string Severity { get; set; }

    /// <summary> A value indicating whether or not the content has been filtered. </summary>
    [JsonPropertyName("filtered")]
    public bool Filtered { get; set; }
}