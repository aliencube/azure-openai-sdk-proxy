using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the response entity for access code.
/// </summary>
public class AccessCodeResponse : AccessCodeRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessCodeResponse"/> class.
    /// </summary>
    public AccessCodeResponse()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessCodeResponse"/> class.
    /// </summary>
    /// <param name="record"><see cref="AccessCodeRecord"/> instance.</param>
    public AccessCodeResponse(AccessCodeRecord record)
    {
        this.Id = record.AccessCodeId;
        this.AccessCode = record.ApiKey;
        this.EventId = record.EventId;
        this.Name = record.Name;
        this.Email = record.Email;
        this.GitHubAlias = record.GitHubAlias;
        this.EventDateStart = record.EventDateStart;
        this.EventDateEnd = record.EventDateEnd;
        this.DateCreated = record.DateCreated;
        this.MaxTokens = record.MaxTokens;
    }

    /// <summary>
    /// Gets or sets the access code ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the access code to the OpenAI API.
    /// </summary>
    [JsonPropertyName("accessCode")]
    public string? AccessCode { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens to use.
    /// </summary>
    [JsonPropertyName("maxTokens")]
    public int? MaxTokens { get; set; }
}