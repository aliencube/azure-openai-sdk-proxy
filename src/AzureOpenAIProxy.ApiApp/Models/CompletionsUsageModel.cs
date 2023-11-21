using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// Representation of the token counts processed for a completions request.
/// Counts consider all tokens across prompts, choices, choice alternates, best_of generations, and
/// other consumers.
public class CompletionsUsageModel
{
    /// <summary> The number of tokens generated across all completions emissions. </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    /// <summary> The number of tokens in the provided prompts for the completions request. </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    /// <summary> The total number of tokens processed for the completions request and response. </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}
