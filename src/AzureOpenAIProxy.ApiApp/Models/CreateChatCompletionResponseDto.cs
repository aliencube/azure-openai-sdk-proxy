using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// The response from creating a chat completion.
/// For more information, see <a href="https://github.com/Azure/azure-rest-api-specs/blob/main/specification/cognitiveservices/data-plane/AzureOpenAI/inference/stable/2024-06-01/inference.json">azure-rest-api-specs(2024-06-01)</a>
/// </summary>
/// <param name="Id"></param>
/// <param name="Object"><see cref="ChatCompletionResponseObject"></param>
/// <param name="Created">The Unix timestamp (in seconds) of when the chat completion was created.</param>
/// <param name="Model">The model used for the chat completion.</param>
/// <param name="Usage"><see cref="CompletionUsage"></param>
/// <param name="SystemFingerprint">Can be used in conjunction with the `seed` request parameter to understand when backend changes have been made that might impact determinism.</param>
/// <param name="PromptFilterResults"><see cref="PromptFilterResult"></param>
/// <param name="Choices"><see cref="ChatCompletionChoice"></param>
public record CreateChatCompletionResponseDto(
    [property: JsonPropertyName("id"), Required] string Id,
    [property: JsonPropertyName("object"), Required] ChatCompletionResponseObject Object,
    [property: JsonPropertyName("created"), Required] DateTimeOffset Created,
    [property: JsonPropertyName("model"), Required] string Model,
    [property: JsonPropertyName("usage")] CompletionUsage Usage,
    [property: JsonPropertyName("system_fingerprint")] string SystemFingerprint,
    [property: JsonPropertyName("prompt_filter_results")] List<PromptFilterResult> PromptFilterResults,
    [property: JsonPropertyName("choices"), Required] List<ChatCompletionChoice> Choices
);

/// <summary>
/// Represents a choice in the chat completion response.
/// </summary>
/// <param name="Index"></param>
/// <param name="FinishReason"></param>
/// <param name="Message"><see cref="ChatCompletionResponseMessage"></param>
/// <param name="ContentFilterResults"><see cref="ContentFilterChoiceResults"></param>
/// <param name="Logprobs"><see cref="ChatCompletionChoiceLogProbs"></param>
public record ChatCompletionChoice(
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("finish_reason")] string FinishReason,
    [property: JsonPropertyName("message")] ChatCompletionResponseMessage Message,
    [property: JsonPropertyName("content_filter_results")] ContentFilterChoiceResults ContentFilterResults,
    [property: JsonPropertyName("logprobs")] ChatCompletionChoiceLogProbs Logprobs
);

/// <summary>
/// A chat completion message generated by the model.
/// </summary>
/// <param name="Role"><see cref="ChatCompletionChoiceLogProbs"></param>
/// <param name="Content">The contents of the message.</param>
/// <param name="ToolCalls"><see cref="ChatCompletionMesssageToolCall"></param>
/// <param name="FunctionCall"><see cref="ChatCompletionFunctionCall"></param>
/// <param name="Context"><see cref="AzureChatExtensionsMessageContext"></param>
public record ChatCompletionResponseMessage(
    [property: JsonPropertyName("role")] ChatCompletionResponseMessageRole Role,
    [property: JsonPropertyName("content")] string? Content,
    [property: JsonPropertyName("tool_calls")] List<ChatCompletionMesssageToolCall> ToolCalls,
    [property: JsonPropertyName("function_call")] ChatCompletionFunctionCall FunctionCall,
    [property: JsonPropertyName("context")] AzureChatExtensionsMessageContext Context
);

/// <summary>
/// Information about the content filtering category (hate, sexual, violence, self_harm), 
/// if it has been detected, as well as the severity level (very_low, low, medium, high-scale that determines the intensity and risk level of harmful content) 
/// and if it has been filtered or not. Information about third party text and profanity, if it has been detected, and if it has been filtered or not. 
/// And information about customer block list, if it has been filtered and its id.
/// </summary>
/// <see cref="ContentFilterSeverityResult"/> 
/// <param name="Sexual"></param>
/// <param name="Violence"></param>
/// <param name="Hate"></param>
/// <param name="SelfHarm"></param>
/// <param name="Profanity"><see cref="ContentFilterDetectedResult"/></param>
/// <param name="Error"><see cref="ErrorBase"/> </param>
/// <param name="ProtectedMaterialText"><see cref="ContentFilterDetectedResult"/></param>
/// <param name="ProtectedMaterialCode"><see cref="ContentFilterDetectedWithCitationResult"/></param>
public record ContentFilterChoiceResults(
    [property: JsonPropertyName("sexual")] ContentFilterSeverityResult Sexual,
    [property: JsonPropertyName("violence")] ContentFilterSeverityResult Violence,
    [property: JsonPropertyName("hate")] ContentFilterSeverityResult Hate,
    [property: JsonPropertyName("self_harm")] ContentFilterSeverityResult SelfHarm,
    [property: JsonPropertyName("profanity")] ContentFilterDetectedResult Profanity,
    [property: JsonPropertyName("error")] ErrorBase Error,
    [property: JsonPropertyName("protected_material_text")] ContentFilterDetectedResult ProtectedMaterialText,
    [property: JsonPropertyName("protected_material_code")] ContentFilterDetectedWithCitationResult ProtectedMaterialCode
);

/// <summary>
/// Log probability information for the choice.
/// </summary>
/// <see cref="ChatCompletionTokenLogprob">
/// <param name="Content">A list of message content tokens with log probability information.</param>
public record ChatCompletionChoiceLogProbs(
    [property: JsonPropertyName("content"), Required] List<ChatCompletionTokenLogprob>? Content
);


/// <summary>
/// Usage statistics for the completion request.
/// </summary>
/// <param name="PromptTokens">Number of tokens in the prompt.</param>
/// <param name="CompletionTokens">Number of tokens in the generated completion.</param>
/// <param name="TotalTokens">Total number of tokens used in the request (prompt + completion).</param>
public record CompletionUsage(
    [property: JsonPropertyName("prompt_tokens"), Required] int PromptTokens,
    [property: JsonPropertyName("completion_tokens"), Required] int CompletionTokens,
    [property: JsonPropertyName("total_tokens"), Required] int TotalTokens
);

/// <summary>
/// Content filtering results for a single prompt in the request.
/// </summary>
/// <param name="PromptIndex"></param>
/// <param name="ContentFilterResults"><see cref="ContentFilterPromptResults"/></param>
public record PromptFilterResult(
    [property: JsonPropertyName("prompt_index")] int PromptIndex,
    [property: JsonPropertyName("content_filter_results")] ContentFilterPromptResults ContentFilterResults
);

/// <summary>
/// Represents a tool call generated by the model.
/// </summary>
/// <param name="Id">The ID of the tool call.</param>
/// <param name="Type"><see cref="ToolCallType"/></param>
/// <param name="Function"><see cref="FunctionObject"/></param>
public record ChatCompletionMesssageToolCall(
    [property: JsonPropertyName("id"), Required] string Id,
    [property: JsonPropertyName("type"), Required] ToolCallType Type,
    [property: JsonPropertyName("function"), Required] FunctionObject Function
);

/// <summary>
/// The function that the model called.
/// </summary>
/// <param name="Name">The name of the function to call.</param>
/// <param name="Arguments">The arguments to call the function with, as generated by the model in JSON format. Note that the model does not always generate valid JSON, and may hallucinate parameters not defined by your function schema. Validate the arguments in your code before calling your function.</param>
public record FunctionObject(
    [property: JsonPropertyName("name"), Required] string Name,
    [property: JsonPropertyName("arguments"), Required] string Arguments
);

/// <summary>
/// Deprecated and replaced by `tool_calls`. 
/// The name and arguments of a function that should be called, as generated by the model.
/// </summary>
/// <param name="Name">The name of the function to call.</param>
/// <param name="Arguments">The arguments to call the function with, as generated by the model in JSON format. Note that the model does not always generate valid JSON, and may hallucinate parameters not defined by your function schema. Validate the arguments in your code before calling your function.</param>
public record ChatCompletionFunctionCall(
    [property: JsonPropertyName("name"), Required] string Name,
    [property: JsonPropertyName("arguments"), Required] string Arguments
);

/// <summary>
/// A representation of the additional context information available when Azure OpenAI chat extensions are involved
/// in the generation of a corresponding chat completions response. This context information is only populated when
/// using an Azure OpenAI request configured to use a matching extension.
/// </summary>
/// <see cref="Citation"/>
/// <param name="Citations">The data source retrieval result, used to generate the assistant message in the response.</param>
/// <param name="Intent">The detected intent from the chat history, used to pass to the next turn to carry over the context.</param>
public record AzureChatExtensionsMessageContext(
    [property: JsonPropertyName("citations")] List<Citation> Citations,
    [property: JsonPropertyName("intent")] string Intent
);

/// <summary>
/// Content filtering results with citation information.
/// </summary>
/// <param name="Filtered"></param>
/// <param name="Detected"></param>
/// <param name="Citation"><see cref="CitationObject"/></param>
public record ContentFilterDetectedWithCitationResult(
    [property: JsonPropertyName("filtered"), Required] bool Filtered,
    [property: JsonPropertyName("detected"), Required] bool Detected,
    [property: JsonPropertyName("citation")] CitationObject Citation
);

/// <summary>
/// Citation object within a content filtering result.
/// </summary>
/// <param name="URL"></param>
/// <param name="License"></param>
public record CitationObject(
    [property: JsonPropertyName("URL")] string URL,
    [property: JsonPropertyName("license")] string License
);
/// <summary>
/// Token log probability information.
/// </summary>
/// <param name="Token"></param>
/// <param name="Logprob"></param>
/// <param name="Bytes"></param>
/// <param name="TopLogprobs"><see cref="TopLogprobs"/></param>
public record ChatCompletionTokenLogprob(
    [property: JsonPropertyName("token"), Required] string Token,
    [property: JsonPropertyName("logprob"), Required] decimal Logprob,
    [property: JsonPropertyName("bytes"), Required] List<int>? Bytes,
    [property: JsonPropertyName("top_logprob"), Required] List<TopLogprobs> TopLogprobs
);

/// <summary>
/// List of the most likely tokens and their log probability, at this token position. 
/// In rare cases, there may be fewer than the number of requested `top_logprobs` returned.
/// </summary>
/// <param name="Token">The token.</param>
/// <param name="Logprob">The log probability of this token.</param>
/// <param name="Bytes">A list of integers representing the UTF-8 bytes representation of the token. Useful in instances where characters are represented by multiple tokens and their byte representations must be combined to generate the correct text representation. Can be `null` if there is no bytes representation for the token.</param>
public record TopLogprobs(
    [property: JsonPropertyName("token"), Required] string Token,
    [property: JsonPropertyName("logprob"), Required] decimal Logprob,
    [property: JsonPropertyName("bytes"), Required] List<int>? Bytes
);

/// <summary>
/// Information about the content filtering category (hate, sexual, violence, self_harm), 
/// if it has been detected, as well as the severity level (very_low, low, medium, high-scale that determines the intensity and risk level of harmful content)
/// and if it has been filtered or not. Information about jailbreak content and profanity, if it has been detected, and if it has been filtered or not. 
/// And information about customer block list, if it has been filtered and its id.
/// </summary>
/// <see cref="ContentFilterSeverityResult"/> 
/// <param name="Sexual"></param>
/// <param name="Violence"></param>
/// <param name="Hate"></param>
/// <param name="SelfHarm"></param>
/// <param name="Profanity"><see cref="ContentFilterDetectedResult"/></param>
/// <param name="Error"><see cref="ErrorBase"/></param>
/// <param name="Jailbreak"><see cref="ContentFilterDetectedResult"/></param>
public record ContentFilterPromptResults(
    [property: JsonPropertyName("sexual")] ContentFilterSeverityResult Sexual,
    [property: JsonPropertyName("violence")] ContentFilterSeverityResult Violence,
    [property: JsonPropertyName("hate")] ContentFilterSeverityResult Hate,
    [property: JsonPropertyName("self_harm")] ContentFilterSeverityResult SelfHarm,
    [property: JsonPropertyName("profanity")] ContentFilterDetectedResult Profanity,
    [property: JsonPropertyName("error")] ErrorBase Error,
    [property: JsonPropertyName("jailbreak")] ContentFilterDetectedResult Jailbreak
);

/// <summary>
/// citation information for a chat completions response message.
/// </summary>
/// <param name="Content">The content of the citation.</param>
/// <param name="Title">The title of the citation.</param>
/// <param name="Url">The URL of the citation.</param>
/// <param name="Filepath">The file path of the citation.</param>
/// <param name="ChunkId">The chunk ID of the citation.</param>
public record Citation(
    [property: JsonPropertyName("content"), Required] string Content,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("filepath")] string Filepath,
    [property: JsonPropertyName("chunk_id")] string ChunkId
);

/// <summary>
/// Content filtering result details.
/// </summary>
/// <param name="Filtered"></param>
/// <param name="Detected"></param>
public record ContentFilterDetectedResult(
    [property: JsonPropertyName("filtered")] bool Filtered,
    [property: JsonPropertyName("detected")] bool Detected
);

/// <summary>
/// Severity information for content filtering.
/// </summary>
/// <param name="Filtered"></param>
/// <param name="Severity"><see cref="ContentFilterSeverity"/></param>
public record ContentFilterSeverityResult(
    [property: JsonPropertyName("filtered"), Required] bool Filtered,
    [property: JsonPropertyName("severity"), Required] ContentFilterSeverity Severity
);

/// <summary>
/// Error details for content filtering.
/// </summary>
/// <param name="Code"></param>
/// <param name="Message"></param>
public record ErrorBase(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("message")] string Message
);

/// <summary>
/// The type of the tool call, in this case `function`.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ToolCallType
{
    /// <summary>
    /// The tool call type is function.
    /// </summary>
    [EnumMember(Value = "function")] Function
}

/// <summary>
/// The role of the author of the response message.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChatCompletionResponseMessageRole
{
    [EnumMember(Value = "assistant")] Assistant
}

/// <summary>
/// The object type.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChatCompletionResponseObject
{
    /// <summary>
    /// The object type is chat completion.
    /// </summary>
    [EnumMember(Value = "chat.completion")]
    ChatCompletion
}

/// <summary>
/// Severity levels for content filtering.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContentFilterSeverity
{
    /// <summary>
    /// General content or related content in generic or non-harmful contexts.
    /// </summary>
    [EnumMember(Value = "safe")] Safe,
    /// <summary>
    /// Harmful content at a low intensity and risk level.
    /// </summary>
    [EnumMember(Value = "low")] Low,
    /// <summary>
    /// Harmful content at a medium intensity and risk level.
    /// </summary>
    [EnumMember(Value = "medium")] Medium,
    /// <summary>
    /// Harmful content at a high intensity and risk level.
    /// </summary>
    [EnumMember(Value = "high")] High
}
