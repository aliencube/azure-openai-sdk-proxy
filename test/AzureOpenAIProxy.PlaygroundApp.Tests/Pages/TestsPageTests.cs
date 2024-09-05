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

    [Test]
    public async Task Given_Null_ApiKeyInput_When_DebugButton_Clicked_Then_Should_Show_NullMessage()
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field#api-key-input").Locator("input");
        var debugButton = Page.Locator("fluent-button#debug-api-key");

        // Act
        await apiKeyInput.FillAsync("");
        await debugButton.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync("Input is null.");
    }


    [Test]
    [TestCase("test-api-key-321")]
    [TestCase("example-key-123")]
    public async Task Given_ApiKeyInput_When_Input_Entered_Then_It_Should_Show_Input(string apiKey)
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field#api-key-input").Locator("input");
        var debugButton = Page.Locator("fluent-button#debug-api-key");

        // Act
        await apiKeyInput.FillAsync(apiKey);
        await debugButton.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{apiKey} (Type: System.String)");
    }

}