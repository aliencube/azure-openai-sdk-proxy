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
    public void Given_EnumMemberAttribute_When_Deserializing_Then_ShouldReturnCorrectEnumValue()
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
    public void Given_DotInEnumMemberAttribute_When_Deserializing_Then_ShouldReturnCorrectEnumValue()
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
    public void Given_ThirdEnumMemberAttribute_When_Deserializing_Then_ShouldReturnCorrectEnumValue()
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
    public void Given_NoEnumMemberAttribute_When_Deserializing_Then_ShouldReturnCorrectEnumValue()
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
    public void Given_InvalidEnumValue_When_Deserializing_Then_ShouldThrowJsonException()
    {
        // Arrange
        var json = "\"invalid_value\"";

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };

        // Assert
        Action action = () => JsonSerializer.Deserialize<TestEnum>(json, options);
        action.Should().Throw<JsonException>()
              .WithMessage("Unable to convert \"invalid_value\" to Enum*");
    }

    [Fact]
    public void Given_NullValue_When_Deserializing_Then_ShouldThrowJsonException()
    {
        // Arrange
        var json = "null";

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { _converter }
        };

        // Assert
        Action action = () => JsonSerializer.Deserialize<TestEnum>(json, options);
        action.Should().Throw<JsonException>()
              .WithMessage("Unable to convert null to Enum*");
    }

    [Fact]
    public void Given_EnumMemberAttribute_When_Serializing_Then_ShouldReturnCorrectJsonString()
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
    public void Given_DotInEnumMemberAttribute_When_Serializing_Then_ShouldReturnCorrectJsonString()
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
    public void Given_ThirdEnumMemberAttribute_When_Serializing_Then_ShouldReturnCorrectJsonString()
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
    public void Given_NoEnumMemberAttribute_When_Serializing_Then_ShouldReturnCorrectJsonString()
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