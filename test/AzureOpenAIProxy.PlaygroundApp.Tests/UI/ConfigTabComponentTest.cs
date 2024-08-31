using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.UI;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class ConfigTabComponentTest : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        IgnoreHTTPSErrors = true,
    };  
    
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
        var sysMsgPanel = Page.Locator("fluent-tab-panel#system-message-tab-panel");
        var parameterPanel = Page.Locator("fluent-tab-panel#parameters-tab-panel");
        
        // Assert
        await Expect(sysMsgPanel).ToBeVisibleAsync();
        await Expect(parameterPanel).ToBeHiddenAsync();
    }

    [Test]
    public async Task Given_ConfigTab_When_Changed_Then_Tab_Should_Be_Updated()
    {
        // Arrange
        var parameterTab = Page.Locator("fluent-tab#parameters-tab");
        var sysMsgPanel = Page.Locator("fluent-tab-panel#system-message-tab-panel");
        var parameterPanel = Page.Locator("fluent-tab-panel#parameters-tab-panel");
        
        // Act
        await parameterTab.ClickAsync();
        
        // Assert
        await Expect(sysMsgPanel).ToBeHiddenAsync();
        await Expect(parameterPanel).ToBeVisibleAsync();
    }
}