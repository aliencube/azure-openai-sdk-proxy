using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
///   A representation of the additional context information available when Azure OpenAI chat extensions are involved
///   in the generation of a corresponding chat completions response. This context information is only populated when
///   using an Azure OpenAI request configured to use a matching extension.
/// </summary>
public class AzureChatExtensionsMessageContextModel
{
    /// <summary>
    ///   The contextual message payload associated with the Azure chat extensions used for a chat completions request.
    ///   These messages describe the data source retrievals, plugin invocations, and other intermediate steps taken in the
    ///   course of generating a chat completions response that was augmented by capabilities from Azure OpenAI chat
    ///   extensions.
    /// </summary>
    [JsonPropertyName("messages")]
    public IList<ChatMessageModel> Messages { get; } = new List<ChatMessageModel>();
}
