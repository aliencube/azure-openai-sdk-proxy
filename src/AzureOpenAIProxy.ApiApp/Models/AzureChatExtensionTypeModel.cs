namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
///   A representation of configuration data for a single Azure OpenAI chat extension. This will be used by a chat
///   completions request that should use Azure OpenAI chat extensions to augment the response behavior.
///   The use of this configuration is compatible only with Azure OpenAI.
/// </summary>
/// <remarks> Initializes a new instance of <see cref="AzureChatExtensionTypeModel"/>. </remarks>
public class AzureChatExtensionTypeModel(string value) : IEquatable<AzureChatExtensionTypeModel>
{
    private readonly string _value = value ?? throw new ArgumentNullException(nameof(value));

    private const string AzureCognitiveSearchValue = "AzureCognitiveSearch";

    /// <summary> Represents the use of Azure Cognitive Search as an Azure OpenAI chat extension. </summary>
    public static AzureChatExtensionTypeModel AzureCognitiveSearch { get; } = new AzureChatExtensionTypeModel(AzureCognitiveSearchValue);

    /// <summary> Determines if two <see cref="AzureChatExtensionTypeModel"/> values are the same. </summary>
    public static bool operator ==(AzureChatExtensionTypeModel left, AzureChatExtensionTypeModel right) => left.Equals(right);

    /// <summary> Determines if two <see cref="AzureChatExtensionTypeModel"/> values are not the same. </summary>
    public static bool operator !=(AzureChatExtensionTypeModel left, AzureChatExtensionTypeModel right) => !left.Equals(right);

    /// <summary> Converts a string to a <see cref="AzureChatExtensionTypeModel"/>. </summary>
    public static implicit operator AzureChatExtensionTypeModel(string value) => new AzureChatExtensionTypeModel(value);

    /// <inheritdoc />
    public override bool Equals(object obj) => obj is AzureChatExtensionTypeModel other && Equals(other);

    /// <inheritdoc />
    public bool Equals(AzureChatExtensionTypeModel other) => string.Equals(this._value, other._value, StringComparison.InvariantCultureIgnoreCase);

    /// <inheritdoc />
    public override int GetHashCode() => this._value?.GetHashCode() ?? 0;

    /// <inheritdoc />
    public override string ToString() => this._value;
}
