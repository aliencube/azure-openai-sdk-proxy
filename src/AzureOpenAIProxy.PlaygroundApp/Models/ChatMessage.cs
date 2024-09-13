namespace AzureOpenAIProxy.PlaygroundApp.Models;

/// <summary>
/// This represents the entity for chat message.
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Gets or sets the message role.
    /// </summary>
    public MessageRole? Role { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string? Message { get; set; }
}

/// <summary>
/// This defines the role of the message.
/// </summary>
public enum MessageRole
{
    /// <summary>
    /// Indicates the role is not defined.
    /// </summary>
    Undefined,

    /// <summary>
    /// Indicates the user role.
    /// </summary>
    User,

    /// <summary>
    /// Indicates the assistant role.
    /// </summary>
    Assistant
}
