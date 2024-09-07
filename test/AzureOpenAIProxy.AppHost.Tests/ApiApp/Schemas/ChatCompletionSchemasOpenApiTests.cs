using System.Text.Json;

using AzureOpenAIProxy.AppHost.Tests.Fixtures;

using FluentAssertions;
namespace AzureOpenAIProxy.AppHost.Tests.ApiApp.Schemas;

/// <summary>
/// It includes tests for required fields, reference validation, enum values, and properties
/// </summary>
public class ChatCompletionSchemasOpenApiTests(AspireAppHostFixture host) : IClassFixture<AspireAppHostFixture>
{
    // Required Fields Validation

    [Fact]
    public async Task Given_ChatCompletionResponseSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var createChatCompletionResponseSchema = schemas.GetProperty("CreateChatCompletionResponse");
        var requiredFields = createChatCompletionResponseSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "id", "object", "created", "model", "choices" });
    }

    [Fact]
    public async Task Given_ChatCompletionChoiceLogProbsSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionChoiceLogProbsSchema = schemas.GetProperty("ChatCompletionChoiceLogProbs");
        var requiredFields = chatCompletionChoiceLogProbsSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "content" });
    }

    [Fact]
    public async Task Given_CompletionUsageSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var completionUsageSchema = schemas.GetProperty("CompletionUsage");
        var requiredFields = completionUsageSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "prompt_tokens", "completion_tokens", "total_tokens" });
    }

    [Fact]
    public async Task Given_ChatCompletionMessageToolCallSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionMessageToolCallSchema = schemas.GetProperty("ChatCompletionMessageToolCall");
        var requiredFields = chatCompletionMessageToolCallSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "id", "type", "function" });
    }

    [Fact]
    public async Task Given_FunctionObjectSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var functionObjectSchema = schemas.GetProperty("FunctionObject");
        var requiredFields = functionObjectSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "name", "arguments" });
    }

    [Fact]
    public async Task Given_ChatCompletionFunctionCallSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionFunctionCallSchema = schemas.GetProperty("ChatCompletionFunctionCall");
        var requiredFields = chatCompletionFunctionCallSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "name", "arguments" });
    }

    [Fact]
    public async Task Given_ContentFilterDetectedResultSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterDetectedResultSchema = schemas.GetProperty("ContentFilterDetectedResult");
        var requiredFields = contentFilterDetectedResultSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "detected", "filtered" });
    }

    [Fact]
    public async Task Given_ContentFilterDetectedWithCitationResultSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterDetectedWithCitationResultSchema = schemas.GetProperty("ContentFilterDetectedWithCitationResult");
        var requiredFields = contentFilterDetectedWithCitationResultSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "detected", "filtered" });
    }

    [Fact]
    public async Task Given_ChatCompletionTokenLogProbSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionTokenLogProbSchema = schemas.GetProperty("ChatCompletionTokenLogProb");
        var requiredFields = chatCompletionTokenLogProbSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "token", "logprob", "bytes", "top_logprobs" });
    }

    [Fact]
    public async Task Given_TopLogProbsSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var topLogProbsSchema = schemas.GetProperty("TopLogProbs");
        var requiredFields = topLogProbsSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "token", "logprob", "bytes" });
    }

    [Fact]
    public async Task Given_CitationSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var citationSchema = schemas.GetProperty("Citation");
        var requiredFields = citationSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "content" });
    }

    [Fact]
    public async Task Given_ContentFilterSeverityResultSchema_When_ValidatingRequiredFields_Then_RequiredFieldsShouldBePresent()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterSeverityResultSchema = schemas.GetProperty("ContentFilterSeverityResult");
        var requiredFields = contentFilterSeverityResultSchema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        requiredFields.Should().Contain(new[] { "severity", "filtered" });
    }

    // $ref Validation

    [Fact]
    public async Task Given_CreateChatCompletionResponseSchema_When_ValidatingPromptFilterResultsRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var createChatCompletionResponseSchema = schemas.GetProperty("CreateChatCompletionResponse");
        var prompt_filter_resultsFields = createChatCompletionResponseSchema.GetProperty("properties")
                                                                            .GetProperty("prompt_filter_results");

        // Assert
        prompt_filter_resultsFields.GetProperty("items").GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/PromptFilterResult");
    }

    [Fact]
    public async Task Given_CreateChatCompletionResponseSchema_When_ValidatingChoicesRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var createChatCompletionResponseSchema = schemas.GetProperty("CreateChatCompletionResponse");
        var choicesFields = createChatCompletionResponseSchema.GetProperty("properties")
                                                              .GetProperty("choices");

        // Assert
        choicesFields.GetProperty("items")
                     .GetProperty("$ref").GetString()
                     .Should().Be("#/components/schemas/ChatCompletionChoice");
    }

    [Fact]
    public async Task Given_ChatCompletionChoiceSchema_When_ValidatingMessageRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionChoiceSchema = schemas.GetProperty("ChatCompletionChoice");
        var messageFields = chatCompletionChoiceSchema.GetProperty("properties")
                                                      .GetProperty("message");

        // Assert
        messageFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ChatCompletionResponseMessage");
    }

    [Fact]
    public async Task Given_ChatCompletionChoiceSchema_When_ValidatingContentFilterResultsRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionChoiceSchema = schemas.GetProperty("ChatCompletionChoice");
        var content_filter_resultsFields = chatCompletionChoiceSchema.GetProperty("properties")
                                                                     .GetProperty("content_filter_results");

        // Assert
        content_filter_resultsFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterChoiceResults");
    }

    [Fact]
    public async Task Given_ChatCompletionChoiceSchema_When_ValidatingLogProbsRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionChoiceSchema = schemas.GetProperty("ChatCompletionChoice");
        var logprobsFields = chatCompletionChoiceSchema.GetProperty("properties")
                                                       .GetProperty("logprobs");

        // Assert
        logprobsFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ChatCompletionChoiceLogProbs");
    }

    [Fact]
    public async Task Given_CreateChatCompletionResponseSchema_When_ValidatingObjectRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var createChatCompletionResponseSchema = schemas.GetProperty("CreateChatCompletionResponse");
        var objectFields = createChatCompletionResponseSchema.GetProperty("properties")
                                                             .GetProperty("object");

        // Assert
        objectFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ChatCompletionResponseObject");
    }

    [Fact]
    public async Task Given_CreateChatCompletionResponseSchema_When_ValidatingUsageRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var createChatCompletionResponseSchema = schemas.GetProperty("CreateChatCompletionResponse");
        var usageFields = createChatCompletionResponseSchema.GetProperty("properties")
                                                            .GetProperty("usage");

        // Assert
        usageFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/CompletionUsage");
    }

    [Fact]
    public async Task Given_ChatCompletionResponseMessageSchema_When_ValidatingRoleRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionResponseMessageSchema = schemas.GetProperty("ChatCompletionResponseMessage");
        var roleFields = chatCompletionResponseMessageSchema.GetProperty("properties")
                                                            .GetProperty("role");

        // Assert
        roleFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ChatCompletionResponseMessageRole");
    }

    [Fact]
    public async Task Given_ChatCompletionResponseMessageSchema_When_ValidatingToolCallsRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionResponseMessageSchema = schemas.GetProperty("ChatCompletionResponseMessage");
        var toolCallsFields = chatCompletionResponseMessageSchema.GetProperty("properties")
                                                                 .GetProperty("tool_calls");

        // Assert
        toolCallsFields.GetProperty("items").GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ChatCompletionMessageToolCall");
    }

    [Fact]
    public async Task Given_ChatCompletionResponseMessageSchema_When_ValidatingFunctionCallRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionResponseMessageSchema = schemas.GetProperty("ChatCompletionResponseMessage");
        var function_callFields = chatCompletionResponseMessageSchema.GetProperty("properties")
                                                                     .GetProperty("function_call");

        // Assert
        function_callFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ChatCompletionFunctionCall");
    }

    [Fact]
    public async Task Given_ChatCompletionResponseMessageSchema_When_ValidatingContextRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionResponseMessageSchema = schemas.GetProperty("ChatCompletionResponseMessage");
        var contextFields = chatCompletionResponseMessageSchema.GetProperty("properties")
                                                               .GetProperty("context");

        // Assert
        contextFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/AzureChatExtensionsMessageContext");
    }

    [Fact]
    public async Task Given_ContentFilterChoiceResultsSchema_When_ValidatingProtectedMaterialTextRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterChoiceResultsSchema = schemas.GetProperty("ContentFilterChoiceResults");
        var protected_material_textFields = contentFilterChoiceResultsSchema.GetProperty("properties")
                                                                            .GetProperty("protected_material_text");

        // Assert
        protected_material_textFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterDetectedResult");
    }

    [Fact]
    public async Task Given_ContentFilterChoiceResultsSchema_When_ValidatingProtectedMaterialCodeRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterChoiceResultsSchema = schemas.GetProperty("ContentFilterChoiceResults");
        var protected_material_codeFields = contentFilterChoiceResultsSchema.GetProperty("properties")
                                                                            .GetProperty("protected_material_code");

        // Assert
        protected_material_codeFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterDetectedWithCitationResult");
    }

    [Fact]
    public async Task Given_ChatCompletionChoiceLogProbsSchema_When_ValidatingContentRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionChoiceLogProbsSchema = schemas.GetProperty("ChatCompletionChoiceLogProbs");
        var contentFields = chatCompletionChoiceLogProbsSchema.GetProperty("properties")
                                                              .GetProperty("content");

        // Assert
        contentFields.GetProperty("items").GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ChatCompletionTokenLogProb");
    }

    [Fact]
    public async Task Given_PromptFilterResultSchema_When_ValidatingContentFilterResultsRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var promptFilterResultSchema = schemas.GetProperty("PromptFilterResult");
        var content_filter_resultsFields = promptFilterResultSchema.GetProperty("properties")
                                                                   .GetProperty("content_filter_results");

        // Assert
        content_filter_resultsFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterPromptResults");
    }

    [Fact]
    public async Task Given_ChatCompletionMessageToolCallSchema_When_ValidatingTypeRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionMessageToolCallSchema = schemas.GetProperty("ChatCompletionMessageToolCall");
        var typeFields = chatCompletionMessageToolCallSchema.GetProperty("properties")
                                                            .GetProperty("type");

        // Assert
        typeFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ToolCallType");
    }

    [Fact]
    public async Task Given_ChatCompletionMessageToolCallSchema_When_ValidatingFunctionRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionMessageToolCallSchema = schemas.GetProperty("ChatCompletionMessageToolCall");
        var functionFields = chatCompletionMessageToolCallSchema.GetProperty("properties")
                                                                .GetProperty("function");

        // Assert
        functionFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/FunctionObject");
    }

    [Fact]
    public async Task Given_AzureChatExtensionsMessageContextSchema_When_ValidatingCitationsRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var azureChatExtensionsMessageContextSchema = schemas.GetProperty("AzureChatExtensionsMessageContext");
        var citationsFields = azureChatExtensionsMessageContextSchema.GetProperty("properties")
                                                                     .GetProperty("citations");

        // Assert
        citationsFields.GetProperty("items").GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/Citation");
    }

    [Fact]
    public async Task Given_ContentFilterPromptResultsSchema_When_ValidatingJailbreakRef_Then_RefShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterPromptResultsSchema = schemas.GetProperty("ContentFilterPromptResults");
        var jailbreakFields = contentFilterPromptResultsSchema.GetProperty("properties")
                                                              .GetProperty("jailbreak");

        // Assert
        jailbreakFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterDetectedResult");
    }

    [Fact]
    public async Task Given_ContentFilterPromptResultsSchema_When_ValidatingSeverityRefs_Then_RefsShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterPromptResultsSchema = schemas.GetProperty("ContentFilterPromptResults");
        var sexualFields = contentFilterPromptResultsSchema.GetProperty("properties")
                                                           .GetProperty("sexual");
        var violenceFields = contentFilterPromptResultsSchema.GetProperty("properties")
                                                             .GetProperty("violence");
        var hateFields = contentFilterPromptResultsSchema.GetProperty("properties")
                                                         .GetProperty("hate");
        var self_harmFields = contentFilterPromptResultsSchema.GetProperty("properties")
                                                              .GetProperty("self_harm");
        var profanityFields = contentFilterPromptResultsSchema.GetProperty("properties")
                                                              .GetProperty("profanity");
        var errorFields = contentFilterPromptResultsSchema.GetProperty("properties")
                                                          .GetProperty("error");

        // Assert
        sexualFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterSeverityResult");
        violenceFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterSeverityResult");
        hateFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterSeverityResult");
        self_harmFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterSeverityResult");
        profanityFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterDetectedResult");
        errorFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ErrorBase");
    }

    [Fact]
    public async Task Given_ContentFilterChoiceResultsSchema_When_ValidatingSeverityRefs_Then_RefsShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterChoiceResultsSchema = schemas.GetProperty("ContentFilterChoiceResults");
        var sexualFields = contentFilterChoiceResultsSchema.GetProperty("properties")
                                                           .GetProperty("sexual");
        var violenceFields = contentFilterChoiceResultsSchema.GetProperty("properties")
                                                             .GetProperty("violence");
        var hateFields = contentFilterChoiceResultsSchema.GetProperty("properties")
                                                         .GetProperty("hate");
        var self_harmFields = contentFilterChoiceResultsSchema.GetProperty("properties")
                                                              .GetProperty("self_harm");
        var profanityFields = contentFilterChoiceResultsSchema.GetProperty("properties")
                                                              .GetProperty("profanity");
        var errorFields = contentFilterChoiceResultsSchema.GetProperty("properties")
                                                          .GetProperty("error");

        // Assert
        sexualFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterSeverityResult");
        violenceFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterSeverityResult");
        hateFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterSeverityResult");
        self_harmFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterSeverityResult");
        profanityFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ContentFilterDetectedResult");
        errorFields.GetProperty("$ref")
        .GetString().Should().Be("#/components/schemas/ErrorBase");
    }

    // Enum validation

    [Fact]
    public async Task Given_ChatCompletionResponseObjectSchema_When_ValidatingEnum_Then_EnumValuesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionResponseObjectSchema = schemas.GetProperty("ChatCompletionResponseObject");
        var enumValues = chatCompletionResponseObjectSchema.GetProperty("enum").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        enumValues.Should().Contain(new[] { "chat.completion" });
    }

    [Fact]
    public async Task Given_ChatCompletionResponseMessageRoleSchema_When_ValidatingEnum_Then_EnumValuesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionResponseMessageRoleSchema = schemas.GetProperty("ChatCompletionResponseMessageRole");
        var enumValues = chatCompletionResponseMessageRoleSchema.GetProperty("enum").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        enumValues.Should().Contain(new[] { "assistant" });
    }

    [Fact]
    public async Task Given_ContentFilterSeveritySchema_When_ValidatingEnum_Then_EnumValuesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterSeveritySchema = schemas.GetProperty("ContentFilterSeverity");
        var enumValues = contentFilterSeveritySchema.GetProperty("enum").EnumerateArray().Select(x => x.GetString()).ToList();

        // Assert
        enumValues.Should().Contain(new[] { "safe", "low", "medium", "high" });
    }

    // Properties validation

    [Fact]
    public async Task Given_CreateChatCompletionResponseSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var createChatCompletionResponseSchema = schemas.GetProperty("CreateChatCompletionResponse");
        var properties = createChatCompletionResponseSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "id", "object", "created", "model", "usage", "system_fingerprint", "prompt_filter_results", "choices" });
    }

    [Fact]
    public async Task Given_ChatCompletionChoiceSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionChoiceSchema = schemas.GetProperty("ChatCompletionChoice");
        var properties = chatCompletionChoiceSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "index", "finish_reason", "message", "content_filter_results", "logprobs" });
    }

    [Fact]
    public async Task Given_ChatCompletionResponseMessageSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionResponseMessageSchema = schemas.GetProperty("ChatCompletionResponseMessage");
        var properties = chatCompletionResponseMessageSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "role", "content", "tool_calls", "function_call", "context" });
    }

    [Fact]
    public async Task Given_ChatCompletionFunctionCallSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionFunctionCallSchema = schemas.GetProperty("ChatCompletionFunctionCall");
        var properties = chatCompletionFunctionCallSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "name", "arguments" });
    }

    [Fact]
    public async Task Given_AzureChatExtensionsMessageContextSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var azureChatExtensionsMessageContextSchema = schemas.GetProperty("AzureChatExtensionsMessageContext");
        var properties = azureChatExtensionsMessageContextSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "citations", "intent" });
    }

    [Fact]
    public async Task Given_ContentFilterSeverityResultSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterSeverityResultSchema = schemas.GetProperty("ContentFilterSeverityResult");
        var properties = contentFilterSeverityResultSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "severity", "filtered" });
    }

    [Fact]
    public async Task Given_CompletionUsageSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var completionUsageSchema = schemas.GetProperty("CompletionUsage");
        var properties = completionUsageSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "prompt_tokens", "completion_tokens", "total_tokens" });
    }

    [Fact]
    public async Task Given_ContentFilterDetectedResultSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterDetectedResultSchema = schemas.GetProperty("ContentFilterDetectedResult");
        var properties = contentFilterDetectedResultSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "filtered", "detected" });
    }

    [Fact]
    public async Task Given_ContentFilterDetectedWithCitationResultSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterDetectedWithCitationResultSchema = schemas.GetProperty("ContentFilterDetectedWithCitationResult");
        var properties = contentFilterDetectedWithCitationResultSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "filtered", "detected", "citation" });
    }

    [Fact]
    public async Task Given_FunctionObjectSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var functionObjectSchema = schemas.GetProperty("FunctionObject");
        var properties = functionObjectSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "name", "arguments" });
    }

    [Fact]
    public async Task Given_ChatCompletionChoiceLogProbsSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionChoiceLogProbsSchema = schemas.GetProperty("ChatCompletionChoiceLogProbs");
        var properties = chatCompletionChoiceLogProbsSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "content" });
    }

    [Fact]
    public async Task Given_ChatCompletionTokenLogProbSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionTokenLogProbSchema = schemas.GetProperty("ChatCompletionTokenLogProb");
        var properties = chatCompletionTokenLogProbSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "token", "logprob", "bytes", "top_logprobs" });
    }

    [Fact]
    public async Task Given_TopLogProbsSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var topLogProbsSchema = schemas.GetProperty("TopLogProbs");
        var properties = topLogProbsSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "token", "logprob", "bytes" });
    }

    [Fact]
    public async Task Given_CitationSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var citationSchema = schemas.GetProperty("Citation");
        var properties = citationSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "content", "title", "url", "filepath", "chunk_id" });
    }

    [Fact]
    public async Task Given_PromptFilterResultSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var promptFilterResultSchema = schemas.GetProperty("PromptFilterResult");
        var properties = promptFilterResultSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "prompt_index", "content_filter_results" });
    }

    [Fact]
    public async Task Given_ContentFilterPromptResultsSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterPromptResultsSchema = schemas.GetProperty("ContentFilterPromptResults");
        var properties = contentFilterPromptResultsSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "sexual", "violence", "hate", "self_harm", "profanity", "error", "jailbreak" });
    }

    [Fact]
    public async Task Given_ContentFilterChoiceResultsSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var contentFilterChoiceResultsSchema = schemas.GetProperty("ContentFilterChoiceResults");
        var properties = contentFilterChoiceResultsSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "sexual", "violence", "hate", "self_harm", "profanity", "error", "protected_material_text", "protected_material_code" });
    }

    [Fact]
    public async Task Given_ChatCompletionMessageToolCallSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var chatCompletionMessageToolCallSchema = schemas.GetProperty("ChatCompletionMessageToolCall");
        var properties = chatCompletionMessageToolCallSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "id", "type", "function" });
    }

    [Fact]
    public async Task Given_ErrorBaseSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var errorBaseSchema = schemas.GetProperty("ErrorBase");
        var properties = errorBaseSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "code", "message" });
    }

    [Fact]
    public async Task Given_CitationObjectSchema_When_ValidatingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        using var httpClient = host.App!.CreateHttpClient("apiapp");
        await host.ResourceNotificationService.WaitForResourceAsync("apiapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var json = await httpClient.GetStringAsync("/swagger/v1.0.0/swagger.json");
        var openapi = JsonSerializer.Deserialize<JsonDocument>(json);
        var schemas = openapi!.RootElement.GetProperty("components").GetProperty("schemas");

        // Act
        var citationObjectSchema = schemas.GetProperty("CitationObject");
        var properties = citationObjectSchema.GetProperty("properties").EnumerateObject().Select(x => x.Name).ToList();

        // Assert
        properties.Should().Contain(new[] { "URL", "license" });
    }
}