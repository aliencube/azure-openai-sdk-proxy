namespace AzureOpenAIProxy.WebApp.Clients;

public class OpenAIApiClientOptions
{
    public string? Endpoint { get; set; }
    public string? ApiKey { get; set; }
    public string? DeploymentName { get; set; }

    public string? SystemPrompt { get; set; }

    public string? UserPrompt { get; set; }

    public int? MaxToken { get; set; } = 4096;

    public float? Temperature { get; set; } = 0.7f;
}