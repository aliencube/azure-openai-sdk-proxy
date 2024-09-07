using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureOpenAIProxy.ApiApp.Converters;

/// <summary>
/// This represents the converter entity for <see cref="EnumMemberAttribute"/>.
/// </summary>
/// <typeparam name="T">The type of the enum to be converted.</typeparam>
public class EnumMemberConverter<T> : JsonConverter<T> where T : Enum
{
    /// <inheritdoc />
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var enumText = reader.GetString();

        if (enumText == null)
        {
            throw new JsonException($"Unable to convert null to Enum \"{typeToConvert}\".");
        }

        foreach (var field in typeToConvert.GetFields())
        {
            var attribute = Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) as EnumMemberAttribute;

            if (attribute != null && attribute.Value == enumText)
            {
                var value = field.GetValue(null);
                if (value != null)
                {
                    return (T)value;
                }
            }
            else if (field.Name == enumText)
            {
                var value = field.GetValue(null);
                if (value != null)
                {
                    return (T)value;
                }
            }
        }

        throw new JsonException($"Unable to convert \"{enumText}\" to Enum \"{typeToConvert}\".");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            var attribute = Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) as EnumMemberAttribute;

            if (attribute != null)
            {
                writer.WriteStringValue(attribute.Value);
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }

}