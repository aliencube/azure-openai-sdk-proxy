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

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}
