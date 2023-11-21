using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary> The definition of a caller-specified function that chat completions may invoke in response to matching user input. </summary>
public class FunctionDefinitionModel
{
    /// <summary> Initializes a new instance of <see cref="FunctionDefinitionModel"/>.</summary>
    public FunctionDefinitionModel()
    {
    }

    /// <summary> Initializes a new instance of <see cref="FunctionDefinitionModel"/>.</summary>
    /// <param name="name"> The name of the function to be called. </param>
    public FunctionDefinitionModel(string name)
    {
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary> The name of the function to be called. </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// A description of what the function does. The model will use this description when selecting the function and
    /// interpreting its parameters.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// The parameters the functions accepts, described as a JSON Schema object.
    /// <para>
    /// To assign an object to this property use <see cref="BinaryData.FromObjectAsJson{T}(T, System.Text.Json.JsonSerializerOptions?)"/>.
    /// </para>
    /// <para>
    /// To assign an already formatted json string to this property use <see cref="BinaryData.FromString(string)"/>.
    /// </para>
    /// <para>
    /// Examples:
    /// <list type="bullet">
    /// <item>
    /// <term>BinaryData.FromObjectAsJson("foo")</term>
    /// <description>Creates a payload of "foo".</description>
    /// </item>
    /// <item>
    /// <term>BinaryData.FromString("\"foo\"")</term>
    /// <description>Creates a payload of "foo".</description>
    /// </item>
    /// <item>
    /// <term>BinaryData.FromObjectAsJson(new { key = "value" })</term>
    /// <description>Creates a payload of { "key": "value" }.</description>
    /// </item>
    /// <item>
    /// <term>BinaryData.FromString("{\"key\": \"value\"}")</term>
    /// <description>Creates a payload of { "key": "value" }.</description>
    /// </item>
    /// </list>
    /// </para>
    /// </summary>
    [JsonPropertyName("parameters")]
    public BinaryData Parameters { get; set; }

    internal bool IsPredefined { get; set; } = false;

    /// <inheritdoc cref="FunctionCallPresetModel.Auto"/>
    public static FunctionDefinitionModel Auto
        = CreatePredefinedFunctionDefinition(FunctionCallPresetModel.Auto.ToString());

    /// <inheritdoc cref="FunctionCallPresetModel.None"/>
    public static FunctionDefinitionModel None
        = CreatePredefinedFunctionDefinition(FunctionCallPresetModel.None.ToString());

    internal static FunctionDefinitionModel CreatePredefinedFunctionDefinition(string functionName)
        => new(functionName)
        {
            IsPredefined = true
        };
}
