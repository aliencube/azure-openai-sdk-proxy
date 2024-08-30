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
        [JsonPropertyName("role")]
        public ChatCompletionRequestMessageRole Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }

    public class UserChatCompletionRequestMessage : ChatCompletionRequestMessage
    {
        public UserChatCompletionRequestMessage()
        {
            Role = ChatCompletionRequestMessageRole.User;
        }
    }

    public class AssistantChatCompletionRequestMessage : ChatCompletionRequestMessage
    {
        public AssistantChatCompletionRequestMessage()
        {
            Role = ChatCompletionRequestMessageRole.Assistant;
        }
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
}
