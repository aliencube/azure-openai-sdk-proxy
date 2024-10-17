using Azure.Data.Tables;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using AzureOpenAIProxy.ApiApp.Builders;
using AzureOpenAIProxy.ApiApp.Configurations;
using AzureOpenAIProxy.ApiApp.Filters;
using AzureOpenAIProxy.ApiApp.Services;

using Microsoft.OpenApi.Models;

namespace AzureOpenAIProxy.ApiApp.Extensions;

/// <summary>
/// This represents the extension class for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the KeyVault service to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddKeyVaultService(this IServiceCollection services)
    {
        services.AddSingleton<SecretClient>(sp =>
        {
            var configuration = sp.GetService<IConfiguration>()
                ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

            var settings = configuration.GetSection(AzureSettings.Name).GetSection(KeyVaultSettings.Name).Get<KeyVaultSettings>()
                ?? throw new InvalidOperationException($"{nameof(KeyVaultSettings)} could not be retrieved from the configuration.");

            if (string.IsNullOrWhiteSpace(settings.VaultUri) == true)
            {
                throw new InvalidOperationException($"{nameof(KeyVaultSettings.VaultUri)} is not defined.");
            }

            if (string.IsNullOrWhiteSpace(settings.SecretNames[KeyVaultSecretNames.OpenAI]) == true)
            {
                throw new InvalidOperationException($"{nameof(KeyVaultSettings.SecretNames)}.{KeyVaultSecretNames.OpenAI} is not defined.");
            }

            var client = new SecretClient(new Uri(settings.VaultUri), new DefaultAzureCredential());

            return client;
        });

        return services;
    }

    /// <summary>
    /// Adds the OpenAI configuration settings to the service collection by reading appsettings.json.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddOpenAISettings(this IServiceCollection services)
    {
        services.AddSingleton<OpenAISettings>(sp =>
        {
            var settings = new OpenAISettingsBuilder()
                               //.WithAppSettings(sp)
                               .WithKeyVault(sp)
                               .Build();

            return settings;
        });

        return services;
    }

    /// <summary>
    /// Adds the OpenAI service to the service collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddOpenAIService(this IServiceCollection services)
    {
        services.AddOpenAISettings();
        services.AddHttpClient<IOpenAIService, OpenAIService>();

        return services;
    }

    /// <summary>
    /// Adds the OpenAPI service to the services collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddOpenApiService(this IServiceCollection services)
    {
        var settings = services.GetOpenApiSettings();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var info = new OpenApiInfo()
            {
                Version = settings.DocVersion,
                Title = Constants.Title,
                Description = "Providing a proxy service to Azure OpenAI API",
                Contact = new OpenApiContact()
                {
                    Name = "Azure OpenAI Proxy API",
                    Email = "aoai-proxy@contoso.com",
                    Url = new Uri("https://aka.ms/aoai-proxy.net")
                },
            };
            options.SwaggerDoc(settings.DocVersion, info);

            options.AddSecurityDefinition(
                "apiKey",
                new OpenApiSecurityScheme()
                {
                    Name = "api-key",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "API key needed to access the endpoints.",
                    In = ParameterLocation.Header,
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "apiKey" }
                    },
                    Array.Empty<string>()
                }
            });

            options.DocumentFilter<OpenApiTagFilter>();
            options.OperationFilter<OpenApiParameterIgnoreFilter>();
        });

        return services;
    }

    /// <summary>
    /// Adds the TableServiceClient to the services collection.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddTableStorageService(this IServiceCollection services)
    {
        services.AddSingleton<TableServiceClient>(sp => {
            var configuration = sp.GetService<IConfiguration>()
                ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");
            
            var settings = configuration.GetSection(AzureSettings.Name).GetSection(KeyVaultSettings.Name).Get<KeyVaultSettings>()
                ?? throw new InvalidOperationException($"{nameof(KeyVaultSettings)} could not be retrieved from the configuration.");

            var clientSecret = sp.GetService<SecretClient>()
                ?? throw new InvalidOperationException($"{nameof(SecretClient)} service is not registered.");

            if (string.IsNullOrWhiteSpace(settings.SecretNames[KeyVaultSecretNames.Storage]) == true)
            {
                throw new InvalidOperationException($"{nameof(KeyVaultSettings.SecretNames)}.{KeyVaultSecretNames.Storage} is not defined.");
            }

            var storageKeyVault = clientSecret.GetSecret(settings.SecretNames[KeyVaultSecretNames.Storage]!);

            var client = new TableServiceClient(storageKeyVault.Value.Value);

            return client;
        });

        return services;
    }

        /// <summary>
    /// Gets the storage account configuration settings by reading appsettings.json.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
    /// <returns>Returns <see cref="StorageAccountSettings"/> instance.</returns>
    public static IServiceCollection AddStorageAccountSettings(this IServiceCollection services)
    {
        services.AddSingleton<StorageAccountSettings>(sp => {
            var configuration = sp.GetService<IConfiguration>()
                ?? throw new InvalidOperationException($"{nameof(IConfiguration)} service is not registered.");

            var settings = configuration.GetSection(AzureSettings.Name).GetSection(StorageAccountSettings.Name).Get<StorageAccountSettings>()
                ?? throw new InvalidOperationException($"{nameof(StorageAccountSettings)} could not be retrieved from the configuration.");

            if (string.IsNullOrWhiteSpace(settings.TableStorage.TableName) == true)
            {
                throw new InvalidOperationException($"{StorageAccountSettings.Name}.{TableStorageSettings.Name} is not defined.");
            }

            return settings;
        });

        return services;
    }
}
