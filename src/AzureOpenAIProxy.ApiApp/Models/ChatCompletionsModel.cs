using System.Text.Json.Serialization;

using Azure.AI.OpenAI;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// Representation of the response data from a chat completions request.
/// Completions support a wide variety of tasks and generate text that continues from or "completes"
/// provided prompt data.
/// </summary>
public class ChatCompletionsModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatCompletionsModel"/> class.
    /// </summary>
    public ChatCompletionsModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatCompletionsModel"/> class.
    /// </summary>
    /// <param name="completions"><see cref="ChatCompletions"/> instance.</param>
    public ChatCompletionsModel(ChatCompletions completions)
    {
        this.Id = completions.Id;
        this.Created = completions.Created;
        this.Choices = completions.Choices.Select(p => new ChatChoiceModel(p)).ToList();
        this.PromptFilterResults = completions.PromptFilterResults;
        this.Usage = completions.Usage;
    }

    /// <summary> A unique identifier associated with this chat completions response. </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The first timestamp associated with generation activity for this completions response,
    /// represented as seconds since the beginning of the Unix epoch of 00:00 on 1 Jan 1970.
    /// </summary>
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// The collection of completions choices associated with this completions response.
    /// Generally, `n` choices are generated per provided prompt with a default value of 1.
    /// Token limits and other settings may limit the number of choices generated.
    /// </summary>
    [JsonPropertyName("choices")]
    public IReadOnlyList<ChatChoiceModel> Choices { get; set; }

    /// <summary>
    /// Content filtering results for zero or more prompts in the request. In a streaming request,
    /// results for different prompts may arrive at different times or in different orders.
    /// </summary>
    [JsonPropertyName("prompt_filter_results")]
    public IReadOnlyList<PromptFilterResultModel> PromptFilterResults { get; set; }

    /// <summary> Usage information for tokens processed and generated as part of this completions operation. </summary>
    [JsonPropertyName("usage")]
    public CompletionsUsageModel Usage { get; set; }
}
