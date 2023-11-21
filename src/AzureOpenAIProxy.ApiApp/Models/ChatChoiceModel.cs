using System.Text.Json.Serialization;

using Azure.AI.OpenAI;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// The representation of a single prompt completion as part of an overall chat completions request.
/// Generally, `n` choices are generated per provided prompt with a default value of 1.
/// Token limits and other settings may limit the number of choices generated.
public class ChatChoiceModel
{
    public ChatChoiceModel()
    {
    }

    public ChatChoiceModel(ChatChoice choice)
    {
        this.StreamingDeltaMessage = choice.InternalStreamingDeltaMessage;
        this.Message = choice.Message;
    }

    // Chat responses include "delta" objects as part of their SSE payloads when streaming is enabled.
    // This internal property facilitates proper deserialization of streamed chat messages.
    [JsonPropertyName("delta")]
    public ChatMessageModel StreamingDeltaMessage { get; set; }

    /// <summary> The chat message for a given chat completions prompt. </summary>
    [JsonPropertyName("message")]
    public ChatMessageModel Message { get; set; }

    /// <summary> The ordered index associated with this chat completions choice. </summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary> The reason that this chat completions choice completed its generated. </summary>
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }

    /// <summary>
    /// Information about the content filtering category (hate, sexual, violence, self_harm), if it
    /// has been detected, as well as the severity level (very_low, low, medium, high-scale that
    /// determines the intensity and risk level of harmful content) and if it has been filtered or not.
    /// </summary>
    [JsonPropertyName("content_filter_results")]
    public ContentFilterResultsModel ContentFilterResults { get; set; }
}
