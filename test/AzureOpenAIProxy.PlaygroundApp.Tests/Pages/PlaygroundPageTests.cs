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
    public async Task SetUp()
    {
        await Page.GotoAsync("https://localhost:5001/playground/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
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
    public async Task Given_ApiKeyInputField_When_Endpoint_Invoked_Should_Be_Displayed()
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field#apiKeyInputField").Locator("input");

        // Act
        var inputType = await apiKeyInput.GetAttributeAsync("type");

        // Assert
        inputType.Should().NotBeNull("Should be password type");

    }

    [Test]
    [TestCase("test-api-key-1")]
    [TestCase("example-key-123")]
    public async Task Given_ApiKeyInputField_When_Changed_Should_Be_Updated(string apiKey)
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field#apiKeyInputField").Locator("input");

        // Act
        await apiKeyInput.FillAsync(apiKey);

        // Assert
        await Expect(apiKeyInput).ToHaveValueAsync(apiKey);
    }
    
}
