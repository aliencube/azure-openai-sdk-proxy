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

    private readonly CreateChatCompletionResponse exampleResponse = new CreateChatCompletionResponse
    {
        Id = "string",
        Object = ChatCompletionResponseObject.ChatCompletion,
        Created = 1620241923,
        Model = "string",
        Usage = new CompletionUsage
        {
            PromptTokens = 0,
            CompletionTokens = 0,
            TotalTokens = 0
        },
        SystemFingerprint = "string",
        PromptFilterResults = new List<PromptFilterResult>
        {
            new PromptFilterResult
            {
                PromptIndex = 0,
                ContentFilterResults = new ContentFilterPromptResults
                {
                    Sexual = new ContentFilterSeverityResult { Filtered = true, Severity = ContentFilterSeverity.Safe },
                    Violence = new ContentFilterSeverityResult { Filtered = true, Severity = ContentFilterSeverity.Safe },
                    Hate = new ContentFilterSeverityResult { Filtered = true, Severity = ContentFilterSeverity.Safe },
                    SelfHarm = new ContentFilterSeverityResult { Filtered = true, Severity = ContentFilterSeverity.Safe },
                    Profanity = new ContentFilterDetectedResult { Filtered = true, Detected = true },
                    Error = new ErrorBase { Code = "string", Message = "string" },
                    Jailbreak = new ContentFilterDetectedResult { Filtered = true, Detected = true }
                }
            }
        },
        Choices = new List<ChatCompletionChoice>
        {
            new ChatCompletionChoice
            {
                Index = 0,
                FinishReason = "string",
                Message = new ChatCompletionResponseMessage
                {
                    Role = ChatCompletionResponseMessageRole.Assistant,
                    Content = "string",
                    ToolCalls = new List<ChatCompletionMessageToolCall>
                    {
                        new ChatCompletionMessageToolCall
                        {
                            Id = "string",
                            Type = ToolCallType.Function,
                            Function = new FunctionObject
                            {
                                Name = "string",
                                Arguments = "string"
                            }
                        }
                    },
                    FunctionCall = new ChatCompletionFunctionCall { Name = "string", Arguments = "string" },
                    Context = new AzureChatExtensionsMessageContext
                    {
                        Citations = new List<Citation>
                        {
                            new Citation
                            {
                                Content = "string",
                                Title = "string",
                                Url = "string",
                                Filepath = "string",
                                ChunkId = "string"
                            }
                        },
                        Intent = "string"
                    }
                },
                ContentFilterResults = new ContentFilterChoiceResults
                {
                    Sexual = new ContentFilterSeverityResult { Filtered = true, Severity = ContentFilterSeverity.Safe },
                    Violence = new ContentFilterSeverityResult { Filtered = true, Severity = ContentFilterSeverity.Safe },
                    Hate = new ContentFilterSeverityResult { Filtered = true, Severity = ContentFilterSeverity.Safe },
                    SelfHarm = new ContentFilterSeverityResult { Filtered = true, Severity = ContentFilterSeverity.Safe },
                    Profanity = new ContentFilterDetectedResult { Filtered = true, Detected = true },
                    Error = new ErrorBase { Code = "string", Message = "string" },
                    ProtectedMaterialText = new ContentFilterDetectedResult { Filtered = true, Detected = true },
                    ProtectedMaterialCode = new ContentFilterDetectedWithCitationResult
                    {
                        Filtered = true,
                        Detected = true,
                        Citation = new CitationObject { URL = "string", License = "string" }
                    }
                },
                LogProbs = new ChatCompletionChoiceLogProbs
                {
                    Content = new List<ChatCompletionTokenLogProb>
                    {
                        new ChatCompletionTokenLogProb
                        {
                            Token = "string",
                            LogProb = 0,
                            Bytes = new List<int> { 0 },
                            TopLogProbs = new List<TopLogProbs>
                            {
                                new TopLogProbs { Token = "string", LogProb = 0, Bytes = new List<int> { 0 } }
                            }
                        }
                    }
                }
            }
        }
    };

    [Fact]
    public void Given_ExampleResponse_When_Serialized_Then_ShouldMatchExpectedJson()
    {
        // Act
        var serializedJson = JsonSerializer.Serialize(exampleResponse, new JsonSerializerOptions { WriteIndented = false });

        // Assert
        serializedJson.Should().Be(exampleJson.Replace("\r", "").Replace("\n", "").Replace(" ", ""));
    }

    [Fact]
    public void Given_ExampleJson_When_Deserialized_Then_ShouldReturnValidObject()
    {
        // Arrange & Act
        var deserializedResponse = JsonSerializer.Deserialize<CreateChatCompletionResponse>(exampleJson);

        // Assert
        deserializedResponse.Should().NotBeNull();
        deserializedResponse.Should().BeEquivalentTo(exampleResponse);
    }
}
