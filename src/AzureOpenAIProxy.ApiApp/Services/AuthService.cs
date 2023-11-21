using Azure.Data.Tables;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="AuthService"/> class.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Validates the API key.
    /// </summary>
    /// <param name="apiKey">API key.</param>
    /// <returns>Returns <c>true</c>, if the API key is valid; otherwise returns <c>false</c>.</returns>
    Task<bool> ValidateAsync(string apiKey);
}

/// <summary>
/// This represents the service entity for authentication. This MUST be inherited.
/// </summary>
/// <param name="client"><see cref="TableServiceClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger"/> instance.</param>
public abstract class AuthService(TableServiceClient client, ILogger logger) : IAuthService
{
    /// <summary>
    /// Gets or sets the <see cref="TableServiceClient"/> instance.
    /// </summary>
    protected TableServiceClient TableServiceClient { get; private set; } = client ?? throw new ArgumentNullException(nameof(client));

    /// <summary>
    /// Gets or sets the <see cref="ILogger"/> instance.
    /// </summary>
    protected ILogger Logger { get; set; } = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public abstract Task<bool> ValidateAsync(string apiKey);
}
