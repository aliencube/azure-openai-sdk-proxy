namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// The collection of predefined behaviors for handling request-provided function information in a chat completions
/// operation.
/// </summary>
/// <remarks> Initializes a new instance of <see cref="FunctionCallPresetModel"/>. </remarks>
public class FunctionCallPresetModel(string value) : IEquatable<FunctionCallPresetModel>
{
    private readonly string _value = value ?? throw new ArgumentNullException(nameof(value));

    private const string AutoValue = "auto";
    private const string NoneValue = "none";

    /// <summary>
    /// Specifies that the model may either use any of the functions provided in this chat completions request or
    /// instead return a standard chat completions response as if no functions were provided.
    /// </summary>
    public static FunctionCallPresetModel Auto { get; } = new FunctionCallPresetModel(AutoValue);

    /// <summary>
    /// Specifies that the model should not respond with a function call and should instead provide a standard chat
    /// completions response. Response content may still be influenced by the provided function information.
    /// </summary>
    public static FunctionCallPresetModel None { get; } = new FunctionCallPresetModel(NoneValue);

    /// <summary> Determines if two <see cref="FunctionCallPresetModel"/> values are the same. </summary>
    public static bool operator ==(FunctionCallPresetModel left, FunctionCallPresetModel right) => left.Equals(right);

    /// <summary> Determines if two <see cref="FunctionCallPresetModel"/> values are not the same. </summary>
    public static bool operator !=(FunctionCallPresetModel left, FunctionCallPresetModel right) => !left.Equals(right);

    /// <summary> Converts a string to a <see cref="FunctionCallPresetModel"/>. </summary>
    public static implicit operator FunctionCallPresetModel(string value) => new(value);

    /// <inheritdoc />
    public override bool Equals(object obj) => obj is FunctionCallPresetModel other && Equals(other);

    /// <inheritdoc />
    public bool Equals(FunctionCallPresetModel other) => string.Equals(this._value, other._value, StringComparison.InvariantCultureIgnoreCase);

    /// <inheritdoc />
    public override int GetHashCode() => this._value?.GetHashCode() ?? 0;

    /// <inheritdoc />
    public override string ToString() => this._value;
}