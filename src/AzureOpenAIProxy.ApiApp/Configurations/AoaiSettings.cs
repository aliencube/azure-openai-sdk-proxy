namespace AzureOpenAIProxy.ApiApp.Configurations;

public class AoaiSettings
{
    public const string Name = "AOAI";

    public Random Random { get; } = new();
    public List<OpenAISettings> Instances { get; set; } = [];
}

public class OpenAISettings
{
    public const string Name = "OpenAI";

    public string? Endpoint { get; set; } = string.Empty;

    public string? ApiKey { get; set; } = string.Empty;
}
