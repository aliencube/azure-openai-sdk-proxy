using FluentAssertions;

using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public partial class PlaygroundPageTests : PageTest
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
    [TestCase(
        "fluent-tab#system-message-tab",
        "fluent-tab-panel#system-message-tab-panel",
        "div#system-message-tab-component"
    )]
    public async Task Given_ConfigTab_When_Selected_Then_Tab_Component_Should_Be_Displayed(
        string selectedTabSelector,
        string selectedPanelSelector,
        string componenetSelector
    )
    {
        // Arrange
        var selectedTab = Page.Locator(selectedTabSelector);
        var selectedPanel = Page.Locator(selectedPanelSelector);
        var component = Page.Locator(componenetSelector);

        // Act
        await selectedTab.ClickAsync();

        // Assert
        await Expect(selectedPanel).ToBeVisibleAsync();
        await Expect(component).ToBeVisibleAsync();
    }

    [Test]
    [TestCase(
        "fluent-tab#system-message-tab",
        "fluent-text-area#system-message-tab-textarea", 
        "You are an AI assistant that helps people find information."
    )]
    public async Task Given_ConfigTab_When_Selected_Then_Tab_Component_Should_Have_Default_Value(
        string selectedTabSelector,
        string componentSelector, 
        object expectedValue
    )
    {
        // Arrange 
        var selectedTab = Page.Locator(selectedTabSelector);

        // Act
        await selectedTab.ClickAsync();
        var actualValue = await Page.Locator(componentSelector).GetAttributeAsync("value");

        // Assert
        actualValue.Should().Be((string)expectedValue);
    }

    [Test]
    public async Task Given_SystemMessageTab_Buttons_When_TextArea_Value_Changed_Then_All_Buttons_Should_Be_Enabled()
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var systemMessageTextAreaControl = Page.Locator("fluent-text-area#system-message-tab-textarea textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await systemMessageTab.ClickAsync();
        await systemMessageTextAreaControl.FillAsync("New system message");
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled") == null;
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled") == null;

        // Assert
        isApplyButtonEnabled.Should().BeTrue();
        isResetButtonEnabled.Should().BeTrue();
    }

    [Test]
    [TestCase("1 New system message 1")]
    [TestCase("2 New system message 2")]
    public async Task Given_SystemMessageTab_ApplyButton_When_Clicked_Then_Changed_TextArea_Value_Should_Be_Applied_As_SystemMessage_And_ApplyButton_Should_Be_Disabled_And_ResetButton_Should_Be_Enabled(
        string expectedValue
    )
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var systemMessageTextArea = Page.Locator("fluent-text-area#system-message-tab-textarea");
        var systemMessageTextAreaControl = Page.Locator("fluent-text-area#system-message-tab-textarea textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await systemMessageTab.ClickAsync();
        await systemMessageTextAreaControl.FillAsync(expectedValue);
        await applyButton.ClickAsync();
        var actualValue = await systemMessageTextArea.GetAttributeAsync("value");
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled") == null;
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled") == null;

        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().BeFalse();
        isResetButtonEnabled.Should().BeTrue();
    }

    [Test]
    [TestCase("You are an AI assistant that helps people find information.")]
    public async Task Given_SystemMessageTab_ApplyButton_When_Clicked_Then_Default_TextArea_Value_Should_Be_Applied_As_SystemMessage_And_All_Buttons_Should_Be_Disabled(
        string expectedValue
    )
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var systemMessageTextArea = Page.Locator("fluent-text-area#system-message-tab-textarea");
        var systemMessageTextAreaControl = Page.Locator("fluent-text-area#system-message-tab-textarea textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await systemMessageTab.ClickAsync();
        await systemMessageTextAreaControl.FillAsync(expectedValue);
        await applyButton.ClickAsync();
        var actualValue = await systemMessageTextArea.GetAttributeAsync("value");
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled") == null;
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled") == null;
        
        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().BeFalse();
        isResetButtonEnabled.Should().BeFalse();
    }

    [Test]
    [TestCase("You are an AI assistant that helps people find information.")]
    public async Task Given_SystemMessageTab_ResetButton_When_Clicked_Then_SystemMessage_And_TextArea_Should_Have_Default_Value_And_All_Buttons_Should_Be_Disabled(
        string expectedValue
    )
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var systemMessageTextArea = Page.Locator("fluent-text-area#system-message-tab-textarea");
        var systemMessageTextAreaControl = Page.Locator("fluent-text-area#system-message-tab-textarea textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await systemMessageTab.ClickAsync();
        await systemMessageTextAreaControl.FillAsync("New system message");
        await applyButton.ClickAsync();
        await resetButton.ClickAsync();
        var actualValue = await systemMessageTextArea.GetAttributeAsync("value");
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled") == null;
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled") == null;
        
        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().BeFalse();
        isResetButtonEnabled.Should().BeFalse();
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

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}
