using AzureOpenAIProxy.ApiApp.Models;



using System.Text.Json;


namespace AzureOpenAIProxy.ApiApp.Tests.Models
{
    public class CreateChatCompletionRequestTests
    {
        [Fact]
        public void Given_ChatCompletionsJson_When_Deserialized_Then_ShouldReturnCreateChatCompletionRequest()
        {
            // Arrange
            var json = @"
            {
                ""messages"": [
                    {
                        ""role"": ""system"",
                        ""content"": ""you are a helpful assistant that talks like a pirate""
                    },
                    {
                        ""role"": ""user"",
                        ""content"": ""can you tell me how to care for a parrot?""
                    }
                ]
            }";

            // Act
            var result = JsonSerializer.Deserialize<CreateChatCompletionRequest>(json);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Messages); // Ensure Messages is not null
            Assert.Equal(2, result.Messages.Count);

            var firstMessage = result.Messages[0];
            Assert.Equal(ChatCompletionRequestMessageRole.System, firstMessage.Role);
            Assert.Equal("you are a helpful assistant that talks like a pirate", firstMessage.Content);

            var secondMessage = result.Messages[1];
            Assert.Equal(ChatCompletionRequestMessageRole.User, secondMessage.Role);
            Assert.Equal("can you tell me how to care for a parrot?", secondMessage.Content);
        }
    }
}
