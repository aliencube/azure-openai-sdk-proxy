using FluentAssertions;

using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class PlaygroundPageTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new() { IgnoreHTTPSErrors = true, };

    [SetUp]
    public async Task Init()
    {
        await Page.GotoAsync("https://localhost:5001/playground/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    public async Task Given_Page_When_Endpoint_Invoked_Then_It_Should_Show_Header()
    {
        // Arrange
        var header = Page.Locator("div.layout")
                         .Locator("header.header")
                         .Locator("div.header-gutters")
                         .Locator("h1");

        // Act
        var headerText = await header.TextContentAsync();

        // Assert
        headerText.Should().Be("Azure OpenAI Proxy Playground");
    }

    [Test]
    [TestCase("config-grid")]
    [TestCase("chat-grid")]
    public async Task Given_Page_When_Endpoint_Invoked_Then_It_Should_Show_Panels(string id)
    {
        // Act
        var panel = Page.Locator($"div.{id}");

        // Assert
        await Expect(panel).ToBeVisibleAsync();
    }

    [Test]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_ConfigTab_Should_Be_Displayed()
    {
        // Act
        var configTab = Page.Locator("div.config-grid")
                            .Locator("fluent-tabs#config-tab");

        // Assert
        await Expect(configTab).ToBeVisibleAsync();
    }

    [Test]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_Id_Should_Be_System_Message_Tab()
    {
        // Act
        var systemMessagePanel = Page.Locator("div.config-grid")
                                     .Locator("fluent-tabs#config-tab")
                                     .Locator("fluent-tab-panel#system-message-tab-panel");
        var parameterPanel = Page.Locator("div.config-grid")
                                 .Locator("fluent-tabs#config-tab")
                                 .Locator("fluent-tab-panel#parameters-tab-panel");

        // Assert
        await Expect(systemMessagePanel).ToBeVisibleAsync();
        await Expect(parameterPanel).ToBeHiddenAsync();
    }

    [Test]
    [TestCase(
        "fluent-tab#parameters-tab",
        "fluent-tab-panel#parameters-tab-panel",
        "fluent-tab-panel#system-message-tab-panel"
    )]
    [TestCase(
        "fluent-tab#system-message-tab",
        "fluent-tab-panel#system-message-tab-panel",
        "fluent-tab-panel#parameters-tab-panel"
    )]
    public async Task Given_ConfigTab_When_Changed_Then_Tab_Should_Be_Updated(
        string selectedTabSelector,
        string selectedPanelSelector,
        string hiddenPanelSelector
    )
    {
        // Arrange
        var selectedTab = Page.Locator("div.config-grid")
                              .Locator("fluent-tabs#config-tab")
                              .Locator(selectedTabSelector);
        var selectedPanel = Page.Locator("div.config-grid")
                                .Locator("fluent-tabs#config-tab")
                                .Locator(selectedPanelSelector);
        var hiddenPanel = Page.Locator("div.config-grid")
                              .Locator("fluent-tabs#config-tab")
                              .Locator(hiddenPanelSelector);

        // Act
        await selectedTab.ClickAsync();

        // Assert
        await Expect(selectedPanel).ToBeVisibleAsync();
        await Expect(hiddenPanel).ToBeHiddenAsync();
    }

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

    [Test]
    public async Task Given_ApiKeynputField_When_Endpoint_Invoked_Then_It_Should_Be_Visible()
    {
        // Arrange
        var id = "api-key";
        var apiKeyInput = Page.Locator($"fluent-text-field#{id}").Locator("input");
        
        // Act & Assert
        await Expect(apiKeyInput).ToBeVisibleAsync();
    }

   [Test]
   public async Task Given_ApiKeyInputField_When_Endpoint_Invoked_Then_It_Should_Be_Password_Type()
    {
        // Arrange
        var id = "api-key";
        var apiKeyInput = Page.Locator($"fluent-text-field#{id}").Locator("input");
        
        // Act
        var inputType = await apiKeyInput.GetAttributeAsync("type");

        // Assert
        inputType.Should().Be("password");
    }

    [Test]
    [TestCase("test-api-key-1")]
    [TestCase("example-key-123")]
    public async Task Given_ApiKeyInputField_When_Changed_Then_It_Should_Be_Updated(string apiKey)
    {
        // Arrange
        var id = "api-key";
        var apiKeyInput = Page.Locator($"fluent-text-field#{id}").Locator("input");

        // Act
        await apiKeyInput.FillAsync(apiKey);

        // Assert
        await Expect(apiKeyInput).ToHaveValueAsync(apiKey);
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}

