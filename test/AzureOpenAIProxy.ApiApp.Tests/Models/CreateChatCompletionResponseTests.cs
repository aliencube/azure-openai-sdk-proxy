using System.Text.Json;
using AzureOpenAIProxy.ApiApp.Models;
using FluentAssertions;

namespace AzureOpenAIProxy.ApiApp.Tests.Models;

public class CreateChatCompletionResponseTests
{
    private readonly string exampleJson = @"
    {
        ""id"": ""string"",
        ""object"": ""chat.completion"",
        ""created"": 1620241923,
        ""model"": ""string"",
        ""usage"": {
            ""prompt_tokens"": 0,
            ""completion_tokens"": 0,
            ""total_tokens"": 0
        },
        ""system_fingerprint"": ""string"",
        ""prompt_filter_results"": [
            {
                ""prompt_index"": 0,
                ""content_filter_results"": {
                    ""sexual"": {
                        ""filtered"": true,
                        ""severity"": ""safe""
                    },
                    ""violence"": {
                        ""filtered"": true,
                        ""severity"": ""safe""
                    },
                    ""hate"": {
                        ""filtered"": true,
                        ""severity"": ""safe""
                    },
                    ""self_harm"": {
                        ""filtered"": true,
                        ""severity"": ""safe""
                    },
                    ""profanity"": {
                        ""filtered"": true,
                        ""detected"": true
                    },
                    ""error"": {
                        ""code"": ""string"",
                        ""message"": ""string""
                    },
                    ""jailbreak"": {
                        ""filtered"": true,
                        ""detected"": true
                    }
                }
            }
        ],
        ""choices"": [
            {
                ""index"": 0,
                ""finish_reason"": ""string"",
                ""message"": {
                    ""role"": ""assistant"",
                    ""content"": ""string"",
                    ""tool_calls"": [
                        {
                            ""id"": ""string"",
                            ""type"": ""function"",
                            ""function"": {
                                ""name"": ""string"",
                                ""arguments"": ""string""
                            }
                        }
                    ],
                    ""function_call"": {
                        ""name"": ""string"",
                        ""arguments"": ""string""
                    },
                    ""context"": {
                        ""citations"": [
                            {
                                ""content"": ""string"",
                                ""title"": ""string"",
                                ""url"": ""string"",
                                ""filepath"": ""string"",
                                ""chunk_id"": ""string""
                            }
                        ],
                        ""intent"": ""string""
                    }
                },
                ""content_filter_results"": {
                    ""sexual"": {
                        ""filtered"": true,
                        ""severity"": ""safe""
                    },
                    ""violence"": {
                        ""filtered"": true,
                        ""severity"": ""safe""
                    },
                    ""hate"": {
                        ""filtered"": true,
                        ""severity"": ""safe""
                    },
                    ""self_harm"": {
                        ""filtered"": true,
                        ""severity"": ""safe""
                    },
                    ""profanity"": {
                        ""filtered"": true,
                        ""detected"": true
                    },
                    ""error"": {
                        ""code"": ""string"",
                        ""message"": ""string""
                    },
                    ""protected_material_text"": {
                        ""filtered"": true,
                        ""detected"": true
                    },
                    ""protected_material_code"": {
                        ""filtered"": true,
                        ""detected"": true,
                        ""citation"": {
                            ""URL"": ""string"",
                            ""license"": ""string""
                        }
                    }
                },
                ""logprobs"": {
                    ""content"": [
                        {
                            ""token"": ""string"",
                            ""logprob"": 0,
                            ""bytes"": [
                                0
                            ],
                            ""top_logprobs"": [
                                {
                                    ""token"": ""string"",
                                    ""logprob"": 0,
                                    ""bytes"": [
                                        0
                                    ]
                                }
                            ]
                        }
                    ]
                }
            }
        ]
    }";

    [Fact]
    public void Given_ExampleJson_When_Deserialized_Then_ShouldReturnValidObject()
    {
        // Arrange & Act
        var deserializedResponse = JsonSerializer.Deserialize<CreateChatCompletionResponse>(exampleJson);

        // Assert
        deserializedResponse.Should().NotBeNull();
    }

    [Fact]
    public void Given_DeserializedObject_When_Serialized_Then_ShouldMatchOriginalJson()
    {
        // Arrange
        var deserializedResponse = JsonSerializer.Deserialize<CreateChatCompletionResponse>(exampleJson);

        // Act
        var serializedJson = JsonSerializer.Serialize(deserializedResponse, new JsonSerializerOptions { WriteIndented = false });

        // Assert
        serializedJson.Should().Be(exampleJson.Replace("\r", "").Replace("\n", "").Replace(" ", ""));
    }
}
