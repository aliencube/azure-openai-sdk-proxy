using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class TestsPageTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        IgnoreHTTPSErrors = true,
    };

    [SetUp]
    public async Task Setup()
    {
        // Arrange
        await Page.GotoAsync("https://localhost:5001/tests");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    [TestCase("debug-target")]
    [TestCase("debug-button")]
    [TestCase("deployment-model-list")]
    [TestCase("debug-button-selected-model")]
    public async Task Given_ComponentID_When_Page_Loaded_Then_Component_Should_Be_Visible(string id)
    {
        // Arrange
        var expectedId = id;

        // Act
        var component = Page.Locator($"#{expectedId}");

        // Assert
        await Expect(component).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task Given_No_Input_On_DebugTarget_When_DebugButton_Clicked_Then_Toast_Should_Show_NullMessage()
    {
        // Arrange
        var button = Page.Locator("fluent-button#debug-button");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync("Input is null.");
    }

    [Test]
    [TestCase(123, typeof(int))]
    [TestCase(456, typeof(int))]
    [TestCase(789, typeof(int))]
    public async Task Given_Input_On_DebugTarget_When_DebugButton_Clicked_Then_Toast_Should_Show_Input(int inputValue, Type inputType)
    {
        // Arrange
        var radio = Page.Locator("fluent-radio-group#debug-target")
                        .Locator($"fluent-radio[current-value='{inputValue}']");
        var button = Page.Locator("fluent-button#debug-button");

        // Act
        await radio.ClickAsync();
        await button.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{inputValue} (Type: {inputType})");
    }
}