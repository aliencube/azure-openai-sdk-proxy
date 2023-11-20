namespace AzureOpenAIProxy.PlaygroundApp.Configurations;

public class OpenAISettings
{
    public const string Name = "OpenAI";

    public string? Endpoint { get; set; } = string.Empty;

    public string? ApiKey { get; set; } = string.Empty;
}
