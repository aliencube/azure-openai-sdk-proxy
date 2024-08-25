using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using FluentAssertions;
using AzureOpenAIProxy.ApiApp.Converters;

namespace AzureOpenAIProxy.ApiApp.Tests.Converters;

public class EnumMemberConverterTests
{
    private enum TestEnum
    {
        [EnumMember(Value = "first_value")]
        FirstValue,

        [EnumMember(Value = "second.value")]
        SecondValue,

        [EnumMember(Value = "thirdvalue")]
        ThirdValue,

        UnmappedValue
    }

    private readonly JsonConverter<TestEnum> _converter = new EnumMemberConverter<TestEnum>();

    [Fact]
    public void Read_ShouldDeserializeEnum_WithEnumMemberAttribute()
    {
        // Arrange
        var json = "\"first_value\"";

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };
        var result = JsonSerializer.Deserialize<TestEnum>(json, options);

        // Assert
        result.Should().Be(TestEnum.FirstValue);
    }

    [Fact]
    public void Read_ShouldDeserializeEnum_WithDotInEnumMemberAttribute()
    {
        // Arrange
        var json = "\"second.value\"";

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };
        var result = JsonSerializer.Deserialize<TestEnum>(json, options);

        // Assert
        result.Should().Be(TestEnum.SecondValue);
    }

    [Fact]
    public void Read_ShouldDeserializeEnum_WithThirdValueEnumMemberAttribute()
    {
        // Arrange
        var json = "\"thirdvalue\"";

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };
        var result = JsonSerializer.Deserialize<TestEnum>(json, options);

        // Assert
        result.Should().Be(TestEnum.ThirdValue);
    }

    [Fact]
    public void Read_ShouldDeserializeEnum_WithoutEnumMemberAttribute()
    {
        // Arrange
        var json = "\"UnmappedValue\"";

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };
        var result = JsonSerializer.Deserialize<TestEnum>(json, options);

        // Assert
        result.Should().Be(TestEnum.UnmappedValue);
    }

    [Fact]
    public void Read_ShouldThrowJsonException_ForInvalidEnumValue()
    {
        // Arrange
        var json = "\"invalid_value\"";

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };

        Action action = () => JsonSerializer.Deserialize<TestEnum>(json, options);

        // Assert
        action.Should().Throw<JsonException>()
              .WithMessage("Unable to convert \"invalid_value\" to Enum*");
    }

    [Fact]
    public void Read_ShouldThrowJsonException_ForNullValue()
    {
        // Arrange
        var json = "null";

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };

        Action action = () => JsonSerializer.Deserialize<TestEnum>(json, options);

        // Assert
        action.Should().Throw<JsonException>()
              .WithMessage("Unable to convert null to Enum*");
    }

    [Fact]
    public void Write_ShouldSerializeEnum_WithEnumMemberAttribute()
    {
        // Arrange
        var value = TestEnum.FirstValue;

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };
        var result = JsonSerializer.Serialize(value, options);

        // Assert
        result.Should().Be("\"first_value\"");
    }

    [Fact]
    public void Write_ShouldSerializeEnum_WithDotInEnumMemberAttribute()
    {
        // Arrange
        var value = TestEnum.SecondValue;

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };
        var result = JsonSerializer.Serialize(value, options);

        // Assert
        result.Should().Be("\"second.value\"");
    }

    [Fact]
    public void Write_ShouldSerializeEnum_WithThirdValueEnumMemberAttribute()
    {
        // Arrange
        var value = TestEnum.ThirdValue;

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };
        var result = JsonSerializer.Serialize(value, options);

        // Assert
        result.Should().Be("\"thirdvalue\"");
    }

    [Fact]
    public void Write_ShouldSerializeEnum_WithoutEnumMemberAttribute()
    {
        // Arrange
        var value = TestEnum.UnmappedValue;

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };
        var result = JsonSerializer.Serialize(value, options);

        // Assert
        result.Should().Be("\"UnmappedValue\"");
    }
}

