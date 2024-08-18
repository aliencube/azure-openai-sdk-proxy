namespace AzureOpenAIProxy.ApiApp.Models;

// Todo: Issue #208 https://github.com/aliencube/azure-openai-sdk-proxy/issues/208
// Fake implementation, just return recieved event id.
public class AdminEventDetails
{
    public string? EventId { get; set; }
}
// Todo: Issue #208