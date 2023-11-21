using AzureOpenAIProxy.PlaygroundApp.Clients;

using Microsoft.AspNetCore.Components;

namespace AzureOpenAIProxy.PlaygroundApp.Components.UI;

public partial class PlaygroundComponent : ComponentBase
{
    /// <summary>
    /// Gets or sets the <see cref="IApiAppClient"/> instance.
    /// </summary>
    [Inject]
    protected IApiAppClient ApiApp { get; set; }

    /// <summary>
    /// Gets or sets the API access code.
    /// </summary>
    protected string? AccessCode { get; set; }

    /// <summary>
    /// Gets or sets the GitHub alias.
    /// </summary>
    protected string? GitHubAlias { get; set; }

    /// <summary>
    /// Gets or sets the OpenAI deployment name.
    /// </summary>
    protected string? DeploymentName { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens to generate.
    /// </summary>
    protected int? MaxTokens { get; set; } = 2048;

    /// <summary>
    /// Gets or sets the temperature for completions.
    /// </summary>
    protected float? Temperature { get; set; } = 0.7f;

    /// <summary>
    /// Gets or sets the prompt to use for completion.
    /// </summary>
    protected string? Prompt { get; set; }

    /// <summary>
    /// Gets or sets the completion result.
    /// </summary>
    protected string? CompletionResult { get; set; }

    /// <summary>
    /// Handles the button click event of <c>Complete</c>.
    /// </summary>
    protected async Task CompleteAsync()
    {
        var result = await ApiApp.GetChatCompletionsAsync(
            this.AccessCode,
            this.GitHubAlias,
            this.DeploymentName,
            this.Prompt,
            this.MaxTokens,
            this.Temperature);

        this.CompletionResult = result;

        await Task.CompletedTask;
    }

    /// <summary>
    /// Handles the button click event of <c>Clear</c>.
    /// </summary>
    protected async Task ClearAsync()
    {
        this.AccessCode = string.Empty;
        this.GitHubAlias = string.Empty;
        this.DeploymentName = string.Empty;
        this.MaxTokens = 2048;
        this.Temperature = 0.7f;
        this.Prompt = string.Empty;
        this.CompletionResult = string.Empty;

        await Task.CompletedTask;
    }
}
