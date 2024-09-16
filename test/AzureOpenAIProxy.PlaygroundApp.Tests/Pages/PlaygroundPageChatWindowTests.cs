using FluentAssertions;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[TestFixture]
[Property("Category", "Integration")]
public partial class PlaygroundPageTests
{
    [Test]
    public async Task Given_ChatGrid_When_Endpoint_Invoked_Then_ChatWindow_Should_Be_Displayed()
    {
        // Act
        var element = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [Test]
    public async Task Given_ChatGrid_When_Endpoint_Invoked_Then_ChatHistory_Should_Be_Empty()
    {
        // Act
        var element = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-history");

        var text = await element.TextContentAsync();

        // Assert
        text.Should().BeNullOrEmpty();
    }

    [Test]
    public async Task Given_ChatGrid_When_Endpoint_Invoked_Then_ChatPrompt_Should_Be_Displayed()
    {
        // Act
        var element = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-prompt");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [Test]
    public async Task Given_ChatGrid_When_Endpoint_Invoked_Then_ChatPromptArea_Should_Be_Displayed()
    {
        // Act
        var element = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-prompt")
                          .Locator("fluent-text-area#prompt");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [Test]
    [TestCase("button-clear")]
    [TestCase("button-send")]
    public async Task Given_ChatGrid_When_Endpoint_Invoked_Then_ChatPromptButton_Should_Be_Displayed(string id)
    {
        // Act
        var element = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-prompt")
                          .Locator($"div > fluent-button#{id}");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [Test]
    [TestCase("abcde")]
    public async Task Given_ChatPrompt_When_Send_Clicked_Then_ChatHistoryMessage_Should_Be_Displayed_OnTheRight(string text)
    {
        // Arrange
        var history = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-history");
        var prompt = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-prompt")
                          .Locator("fluent-text-area#prompt");
        var send = Page.Locator("div.chat-grid")
                       .Locator("div#chat-window")
                       .Locator("div#chat-prompt")
                       .Locator("div > fluent-button#button-send");

        // Act
        await prompt.Locator("textarea").FillAsync(text);
        await send.ClickAsync();

        var style = await history.Locator("div#message-00").GetAttributeAsync("style");

        // Assert
        style.Should().ContainAll("justify-content:", "end");
    }

    [Test]
    [TestCase("abcde")]
    public async Task Given_ChatPrompt_When_Send_Clicked_Then_ChatHistoryMessage_Should_Be_Displayed(string text)
    {
        // Arrange
        var history = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-history");
        var prompt = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-prompt")
                          .Locator("fluent-text-area#prompt");
        var send = Page.Locator("div.chat-grid")
                       .Locator("div#chat-window")
                       .Locator("div#chat-prompt")
                       .Locator("div > fluent-button#button-send");

        // Act
        await prompt.Locator("textarea").FillAsync(text);
        await send.ClickAsync();

        var item = await history.Locator("div#message-00 p").TextContentAsync();

        // Assert
        item.Should().Be(text);
    }

    [Test]
    [TestCase("abcde")]
    public async Task Given_ChatPrompt_When_Send_Clicked_Then_ChatPromptArea_Should_Be_Empty(string text)
    {
        // Arrange
        var prompt = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-prompt")
                          .Locator("fluent-text-area#prompt");
        var send = Page.Locator("div.chat-grid")
                       .Locator("div#chat-window")
                       .Locator("div#chat-prompt")
                       .Locator("div > fluent-button#button-send");

        // Act
        await prompt.Locator("textarea").FillAsync(text);
        await send.ClickAsync();

        var item = await prompt.Locator("textarea").TextContentAsync();

        // Assert
        item.Should().BeNullOrEmpty();
    }

    [Test]
    [TestCase("abcde")]
    public async Task Given_ChatPrompt_When_Clear_Clicked_Then_ChatHistoryMessage_Should_Be_Cleared(string text)
    {
        // Arrange
        var history = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-history");
        var prompt = Page.Locator("div.chat-grid")
                          .Locator("div#chat-window")
                          .Locator("div#chat-prompt")
                          .Locator("fluent-text-area#prompt");
        var send = Page.Locator("div.chat-grid")
                       .Locator("div#chat-window")
                       .Locator("div#chat-prompt")
                       .Locator("div > fluent-button#button-send");
        var clear = Page.Locator("div.chat-grid")
                        .Locator("div#chat-window")
                        .Locator("div#chat-prompt")
                        .Locator("div > fluent-button#button-clear");

        // Act
        await prompt.Locator("textarea").FillAsync(text);
        await send.ClickAsync();
        await clear.ClickAsync();

        var result = await history.TextContentAsync();

        // Assert
        result.Should().BeNullOrEmpty();
    }
}
