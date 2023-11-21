using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary> Content filtering results for a single prompt in the request. </summary>
public class PromptFilterResultModel
{
    /// <summary> The index of this prompt in the set of prompt results. </summary>
    [JsonPropertyName("prompt_index")]
    public int PromptIndex { get; set; }

    /// <summary> Content filtering results for this prompt. </summary>
    [JsonPropertyName("content_filter_results")]
    public ContentFilterResultsModel ContentFilterResults { get; set; }
}
