using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// The configuration information for a chat completions request.
/// Completions support a wide variety of tasks and generate text that continues from or "completes"
/// provided prompt data.
/// </summary>
public class ChatCompletionsOptionsModel
{
    /// <summary>
    /// The collection of context messages associated with this chat completions request.
    /// Typical usage begins with a chat message for the System role that provides instructions for
    /// the behavior of the assistant, followed by alternating messages between the User and
    /// Assistant roles.
    /// </summary>
    [JsonPropertyName("messages")]
    public IList<ChatMessageModel> Messages { get; set; } = new List<ChatMessageModel>();

    /// <inheritdoc cref="CompletionsOptionsModel.ChoicesPerPrompt"/>
    [JsonPropertyName("n")]
    public int? ChoiceCount { get; set; }

    /// <summary>
    /// Gets or sets the deployment name to use for a chat completions request.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When making a request against Azure OpenAI, this should be the customizable name of the "model deployment"
    /// (example: my-gpt4-deployment) and not the name of the model itself (example: gpt-4).
    /// </para>
    /// <para>
    /// When using non-Azure OpenAI, this corresponds to "model" in the request options and should use the
    /// appropriate name of the model (example: gpt-4).
    /// </para>
    /// </remarks>
    [JsonPropertyName("model")]
    public string DeploymentName { get; set; }

    /// <inheritdoc cref="CompletionsOptionsModel.FrequencyPenalty"/>
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }

    /// <inheritdoc cref="CompletionsOptionsModel.MaxTokens"/>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    /// <inheritdoc cref="CompletionsOptionsModel.NucleusSamplingFactor"/>
    [JsonPropertyName("top_p")]
    public float? NucleusSamplingFactor { get; set; }

    /// <inheritdoc cref="CompletionsOptionsModel.PresencePenalty"/>
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }

    /// <inheritdoc cref="CompletionsOptionsModel.StopSequences"/>
    [JsonPropertyName("stop")]
    public IList<string> StopSequences { get; set; } = new List<string>();

    /// <inheritdoc cref="CompletionsOptionsModel.Temperature"/>
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    /// <inheritdoc cref="CompletionsOptionsModel.TokenSelectionBiases"/>
    [JsonPropertyName("logit_bias")]
    public IDictionary<int, int> TokenSelectionBiases { get; set; } = new Dictionary<int, int>();

    /// <inheritdoc cref="CompletionsOptionsModel.User"/>
    [JsonPropertyName("user")]
    public string User { get; set; }

    /// <summary> A list of functions the model may generate JSON inputs for. </summary>
    [JsonPropertyName("functions")]
    public IList<FunctionDefinitionModel> Functions { get; set; } = new List<FunctionDefinitionModel>();

    /// <summary>
    /// Controls how the model will use provided Functions.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///     <item>
    ///         Providing a custom <see cref="FunctionDefinitionModel"/> will request that the model limit its
    ///         completions to function calls for that function.
    ///     </item>
    ///     <item>
    ///         <see cref="Azure.AI.OpenAI.FunctionDefinition.Auto"/> represents the default behavior and will allow the model
    ///         to freely select between issuing a standard completions response or a call to any provided
    ///         function.
    ///     </item>
    ///     <item>
    ///         <see cref="FunctionDefinitionModel.None"/> will request that the model only issue standard
    ///         completions responses, irrespective of provided functions. Note that the function definitions
    ///         provided may still influence the completions content.
    ///     </item>
    ///     </list>
    /// </remarks>
    [JsonPropertyName("function_call")]
    public FunctionDefinitionModel FunctionCall { get; set; }

    /// <summary>
    /// Gets or sets the additional configuration details to use for Azure OpenAI chat completions extensions.
    /// </summary>
    /// <remarks>
    /// These extensions are specific to Azure OpenAI and require use of the Azure OpenAI service.
    /// </remarks>
    public AzureChatExtensionsOptionsModel? AzureExtensionsOptions { get; set; }

    [JsonPropertyName("dataSources")]
    public IList<AzureChatExtensionConfigurationModel> AzureExtensionsDataSources { get; set; } = new List<AzureChatExtensionConfigurationModel>();

    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
}
