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
        var configTab = Page.Locator("fluent-tabs#config-tab");

        // Assert
        await Expect(configTab).ToBeVisibleAsync();
    }

    [Test]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_Id_Should_Be_System_Message_Tab()
    {
        // Act
        var systemMessagePanel = Page.Locator("fluent-tab-panel#system-message-tab-panel");
        var parameterPanel = Page.Locator("fluent-tab-panel#parameters-tab-panel");

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
        var selectedTab = Page.Locator(selectedTabSelector);
        var selectedPanel = Page.Locator(selectedPanelSelector);
        var hiddenPanel = Page.Locator(hiddenPanelSelector);

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
        "fluent-button#debug-button-system-message-tab", 
        "You are an AI assistant that helps people find information.", 
        typeof(string)
    )]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_Tab_Component_Should_Have_Default_Value(
        string selectedTabSelector,
        string debugButtonSelector, 
        string? expectedValue, 
        Type expectedType
    )
    {
        // Arrange 
        var selectedTab = Page.Locator(selectedTabSelector);
        var debugButton = Page.Locator(debugButtonSelector);

        // Act
        await selectedTab.ClickAsync();
        await debugButton.ClickAsync();
        var actualComponentValue = await Page.Locator(".fluent-toast-title").TextContentAsync();

        // Assert
        actualComponentValue.Should().Be($"{expectedValue} (Type: {expectedType})");
    }

    [Test]
    public async Task Given_SystemMessageTab_Buttons_When_TextArea_Value_Changed_Then_All_Buttons_Should_Be_Enabled()
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");
        var newValue = "New system message";

        // Act
        await systemMessageTab.ClickAsync();
        await Page.EvaluateAsync(@$"
                const textarea = document.querySelector('fluent-text-area#system-message-tab-textarea');
                textarea.value = '{newValue}';
                textarea.dispatchEvent(new Event('input', {{ bubbles: true }}));
        ");
        var isApplyButtonDisabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonDisabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        isApplyButtonDisabled.Should().BeNull();
        isResetButtonDisabled.Should().BeNull();
    }

    [Test]
    [TestCase("1 New system message 1", typeof(string))]
    [TestCase("2 New system message 2", typeof(string))]
    public async Task Given_SystemMessageTab_ApplyButton_When_Clicked_Then_Changed_TextArea_Value_Should_Be_Applied_As_SystemMessage_And_ApplyButton_Should_Be_Disabled_And_ResetButton_Should_Be_Enabled(string expectedValue, Type expectedType)
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");
        var debugButton = Page.Locator("fluent-button#debug-button-system-message-tab");

        // Act
        await systemMessageTab.ClickAsync();
        await Page.EvaluateAsync(@$"
                const textarea = document.querySelector('fluent-text-area#system-message-tab-textarea');
                textarea.value = '{expectedValue}';
                textarea.dispatchEvent(new Event('input', {{ bubbles: true }}));
        ");
        await applyButton.ClickAsync();
        await debugButton.ClickAsync();
        var isApplyButtonDisabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonDisabled = await resetButton.GetAttributeAsync("disabled");
        var actualValue = await Page.Locator(".fluent-toast-title").TextContentAsync();

        // Assert
        actualValue.Should().Be($"{expectedValue} (Type: {expectedType})");
        isApplyButtonDisabled.Should().NotBeNull();
        isResetButtonDisabled.Should().BeNull();
    }

    [Test]
    [TestCase("You are an AI assistant that helps people find information.", typeof(string))]
    public async Task Given_SystemMessageTab_ApplyButton_When_Clicked_Then_Default_TextArea_Value_Should_Be_Applied_As_SystemMessage_And_All_Buttons_Should_Be_Disabled(string expectedValue, Type expectedType)
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");
        var debugButton = Page.Locator("fluent-button#debug-button-system-message-tab");

        // Act
        await systemMessageTab.ClickAsync();
        await Page.EvaluateAsync(@$"
                const textarea = document.querySelector('fluent-text-area#system-message-tab-textarea');
                textarea.value = '{expectedValue}';
                textarea.dispatchEvent(new Event('input', {{ bubbles: true }}));
        ");
        await applyButton.ClickAsync();
        await debugButton.ClickAsync();
        var isApplyButtonDisabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonDisabled = await resetButton.GetAttributeAsync("disabled");
        var actualValue = await Page.Locator(".fluent-toast-title").TextContentAsync();

        // Assert
        actualValue.Should().Be($"{expectedValue} (Type: {expectedType})");
        isApplyButtonDisabled.Should().NotBeNull();
        isResetButtonDisabled.Should().NotBeNull();
    }

    [Test]
    [TestCase("You are an AI assistant that helps people find information.", typeof(string))]
    public async Task Given_SystemMessageTab_ResetButton_When_Clicked_Then_SystemMessage_And_TextArea_Should_Have_Default_Value_And_All_Buttons_Should_Be_Disabled(string expectedValue, Type expectedType)
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");
        var debugButton = Page.Locator("fluent-button#debug-button-system-message-tab");
        var newValue = "New system message";

        // Act
        await systemMessageTab.ClickAsync();
        await Page.EvaluateAsync(@$"
                const textarea = document.querySelector('fluent-text-area#system-message-tab-textarea');
                textarea.value = '{newValue}';
                textarea.dispatchEvent(new Event('input', {{ bubbles: true }}));
        ");
        await applyButton.ClickAsync();
        await resetButton.ClickAsync();
        await debugButton.ClickAsync();
        var isApplyButtonDisabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonDisabled = await resetButton.GetAttributeAsync("disabled");
        var actualValue = await Page.Locator(".fluent-toast-title").TextContentAsync();

        // Assert
        actualValue.Should().Be($"{expectedValue} (Type: {expectedType})");
        isApplyButtonDisabled.Should().NotBeNull();
        isResetButtonDisabled.Should().NotBeNull();
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}