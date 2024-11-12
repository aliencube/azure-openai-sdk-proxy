using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AzureOpenAIProxy.ApiApp.Models
{
    public class CreateChatCompletionRequest
    {
        [JsonPropertyName("messages"), Required]
        public List<ChatCompletionRequestMessage>? Messages { get; set; }

    }

    public class ChatCompletionRequestMessage
    {
        [JsonPropertyName("role"), Required]
        public ChatCompletionRequestMessageRole? Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }
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
    }
}
