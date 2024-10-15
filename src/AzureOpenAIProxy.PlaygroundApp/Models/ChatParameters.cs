namespace AzureOpenAIProxy.PlaygroundApp.Models;

public class ChatParameters
{
    public float pastMessages;
    public float maxResponse;
    public float temperature;
    public float topP;

    public IEnumerable<string>? stopSequence;

    public float frequencyPenalty;
    public float presencePenalty;
}