using Azure.AI.OpenAI;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary> A description of the intended purpose of a message within a chat completions interaction. </summary>
public class ChatRoleModel : IEquatable<ChatRoleModel>
{
    private readonly string _value;

    /// <summary> Initializes a new instance of <see cref="ChatRoleModel"/>. </summary>
    public ChatRoleModel()
    { }

    /// <summary> Initializes a new instance of <see cref="ChatRoleModel"/>. </summary>
    /// <param name="value">The value to use.</param>
    public ChatRoleModel(string value)
        => this._value = value ?? throw new ArgumentNullException(nameof(value));

    private const string SystemValue = "system";
    private const string AssistantValue = "assistant";
    private const string UserValue = "user";
    private const string FunctionValue = "function";
    private const string ToolValue = "tool";

    /// <summary> The role that instructs or sets the behavior of the assistant. </summary>
    public static ChatRole System { get; } = new ChatRole(SystemValue);

    /// <summary> The role that provides responses to system-instructed, user-prompted input. </summary>
    public static ChatRole Assistant { get; } = new ChatRole(AssistantValue);

    /// <summary> The role that provides input for chat completions. </summary>
    public static ChatRole User { get; } = new ChatRole(UserValue);

    /// <summary> The role that provides function results for chat completions. </summary>
    public static ChatRole Function { get; } = new ChatRole(FunctionValue);

    /// <summary> The role that represents extension tool activity within a chat completions operation. </summary>
    public static ChatRole Tool { get; } = new ChatRole(ToolValue);

    /// <summary> Determines if two <see cref="ChatRole"/> values are the same. </summary>
    public static bool operator ==(ChatRoleModel left, ChatRoleModel right) => left.Equals(right);

    /// <summary> Determines if two <see cref="ChatRoleModel"/> values are not the same. </summary>
    public static bool operator !=(ChatRoleModel left, ChatRoleModel right) => !left.Equals(right);

    /// <summary> Converts a string to a <see cref="ChatRoleModel"/>. </summary>
    public static implicit operator ChatRoleModel(string value) => new(value);

    /// <inheritdoc />
    public override bool Equals(object obj) => obj is ChatRoleModel other && Equals(other);

    /// <inheritdoc />
    public bool Equals(ChatRoleModel other) => string.Equals(this._value, other._value, StringComparison.InvariantCultureIgnoreCase);

    /// <inheritdoc />
    public override int GetHashCode() => this._value?.GetHashCode() ?? 0;

    /// <inheritdoc />
    public override string ToString() => this._value;
}
