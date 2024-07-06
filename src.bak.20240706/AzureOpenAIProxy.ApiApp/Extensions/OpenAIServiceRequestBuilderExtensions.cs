using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Services;

namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extension entity for <see cref="OpenAIServiceRequestBuilder"/>.
/// </summary>
public static class OpenAIServiceRequestBuilderExtensions
{
    /// <summary>
    /// Builds the <see cref="OpenAIServiceOptions"/> instance.
    /// </summary>
    /// <param name="builder"><see cref="IOpenAIServiceRequestBuilder"/> instance.</param>
    /// <returns>Returns the <see cref="OpenAIServiceOptions"/> instance.</returns>
    public static async Task<OpenAIServiceOptions> BuildAsync(this Task<IOpenAIServiceRequestBuilder> builder)
    {
        var instance = await builder.ConfigureAwait(false);

        return instance.Build();
    }
}
