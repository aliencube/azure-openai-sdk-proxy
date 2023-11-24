using Azure.Data.Tables;

namespace AzureOpenAIProxy.ApiApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="AuthService"/> class.
/// </summary>
/// <typeparam name="T">Type of response implementing <see cref="ITableEntity"/>.</typeparam>
public interface IAuthService<T> where T : ITableEntity
{
    /// <summary>
    /// Validates the API key.
    /// </summary>
    /// <param name="apiKey">API key.</param>
    /// <returns>Returns <c>true</c>, if the API key is valid; otherwise returns <c>false</c>.</returns>
    Task<T> ValidateAsync(string apiKey);
}

/// <summary>
/// This represents the service entity for authentication. This MUST be inherited.
/// </summary>
/// <typeparam name="T">Type of response implementing <see cref="ITableEntity"/>.</typeparam>
/// <param name="client"><see cref="TableServiceClient"/> instance.</param>
/// <param name="logger"><see cref="ILogger"/> instance.</param>
public abstract class AuthService<T> : IAuthService<T> where T : ITableEntity
{
    /// <inheritdoc />
    public abstract Task<T> ValidateAsync(string apiKey);
}
