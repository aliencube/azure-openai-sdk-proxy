using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

public class CreateChatCompletionRequest
{
    [JsonPropertyName("messages"), Required]
    public List<ChatCompletionRequestMessage>? Messages { get; set; }

    [JsonPropertyName("data_sources")]
    public List<AzureChatExtensionConfiguration>? DataSources { get; set; }

    [JsonPropertyName("n")]
    public int? N { get; set; }

    [JsonPropertyName("seed")]
    public long? Seed { get; set; }

    [JsonPropertyName("logprobs")]
    public bool? LogProbs { get; set; }

    [JsonPropertyName("top_logprobs")]
    public int? TopLogProbs { get; set; }

    [JsonPropertyName("response_format")]
    public ChatCompletionResponseFormat? ResponseFormat { get; set; }

    [JsonPropertyName("tools")]
    public List<ChatCompletionTool>? Tools { get; set; }

    [JsonPropertyName("tool_choice")]
    public ChatCompletionToolChoiceOption? ToolChoice { get; set; }

}

public abstract class ChatCompletionRequestMessage
{
    [JsonPropertyName("role")]
    public abstract ChatCompletionRequestMessageRole Role { get; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChatCompletionRequestMessageRole
{
    [JsonPropertyName("system")]
    System,

    [JsonPropertyName("user")]
    User,

    [JsonPropertyName("assistant")]
    Assistant,

    [JsonPropertyName("tool")]
    Tool,

    [JsonPropertyName("function")]
    Function // Deprecated. 
}

public abstract class AzureChatExtensionConfiguration
{
    [JsonPropertyName("type")]
    public abstract AzureChatExtensionType Type { get; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AzureChatExtensionType
{
    [JsonPropertyName("azure_search")]
    AzureSearch,

    [JsonPropertyName("azure_cosmos_db")]
    AzureCosmosDb
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChatCompletionResponseFormat
{
    [JsonPropertyName("text")]
    Text,

    [JsonPropertyName("json_object")]
    JsonObject
}


public class ChatCompletionTool
{
    [JsonPropertyName("type")]
    [Required]
    public ChatCompletionToolType Type { get; set; }

    [JsonPropertyName("function")]
    [Required]
    public ChatCompletionFunction Function { get; set; }
}

public class ChatCompletionFunction
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("name")]
    [Required]
    public string Name { get; set; }

    [JsonPropertyName("parameters")]
    [Required]
    public ChatCompletionFunctionParameters Parameters { get; set; }
}

public class ChatCompletionFunctionParameters
{
    [JsonExtensionData]
    public Dictionary<string, object> AdditionalProperties { get; set; } = new Dictionary<string, object>();
}

public enum ChatCompletionToolChoiceOption
{
    [JsonPropertyName("none")]
    None,

    [JsonPropertyName("auto")]
    Auto,

    [JsonPropertyName("required")]
    Required
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChatCompletionToolType
{
    [JsonPropertyName("function")]
    Function
}