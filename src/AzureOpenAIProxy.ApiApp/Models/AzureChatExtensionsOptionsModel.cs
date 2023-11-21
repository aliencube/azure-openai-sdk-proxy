using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// An abstraction of additional settings used by chat completions to supplement standard behavior with
/// capabilities from configured Azure OpenAI extensions. These capabilities are specific to Azure OpenAI
/// and chat completions requests configured to use them will require use with with that service endpoint.
/// </summary>
public class AzureChatExtensionsOptionsModel
{
    /// <summary>
    /// Gets the collection of data source configurations to use with Azure OpenAI extensions for chat
    /// completions.
    /// </summary>
    [JsonPropertyName("dataSources")]
    public IList<AzureChatExtensionConfigurationModel> Extensions { get; }
}