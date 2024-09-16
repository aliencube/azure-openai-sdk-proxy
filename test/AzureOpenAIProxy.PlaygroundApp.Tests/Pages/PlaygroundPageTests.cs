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
        string expectedValue
    )
    {
        // Arrange 
        var selectedTab = Page.Locator(selectedTabSelector);

        // Act
        await selectedTab.ClickAsync();
        var actualValue = await Page.Locator(componentSelector).GetAttributeAsync("value");

        // Assert
        actualValue.Should().Be(expectedValue);
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
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        isApplyButtonEnabled.Should().BeNull();
        isResetButtonEnabled.Should().BeNull();
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
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().NotBeNull();
        isResetButtonEnabled.Should().BeNull();
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
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled");
        
        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().NotBeNull();
        isResetButtonEnabled.Should().NotBeNull();
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
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled");
        
        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().NotBeNull();
        isResetButtonEnabled.Should().NotBeNull();
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}
