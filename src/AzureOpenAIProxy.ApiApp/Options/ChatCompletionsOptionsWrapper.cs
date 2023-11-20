using Azure.AI.OpenAI;

namespace AzureOpenAIProxy.ApiApp.Options;

public class ChatCompletionsOptionsWrapper : ChatCompletionsOptions
{
    public ChatCompletionsOptionsWrapper()
        : base()
    {
    }

    public ChatCompletionsOptionsWrapper(string deploymentName, IEnumerable<ChatMessage> messages)
        : base(deploymentName, messages)
    {
    }

    public new IList<ChatMessageWrapper> Messages { get; set; } = new List<ChatMessageWrapper>();
}

public class ChatMessageWrapper
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
