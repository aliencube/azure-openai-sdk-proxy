using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.UI;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class DebugButtonTests : PageTest
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
    public async Task Given_Null_Input_When_DebugButton_Clicked_Then_Display_Should_Show_Null_Message()
    {
        // Act
        await Page.GetByRole(AriaRole.Button, new() { Name = "Debug" }).ClickAsync();

        // Assert
        await Expect(Page.Locator("#displayText")).ToHaveTextAsync("Input is null.");
    }

    [Test]
    [TestCase("123")]
    [TestCase("3.14159")]
    [TestCase("Hello, World!")]
    public async Task Given_InputValue_When_DebugButton_Clicked_Then_Display_Should_Show_InputValue(string inputValue)
    {
        // Act
        await Page.GetByLabel($"{inputValue}").CheckAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Debug" }).ClickAsync();

        // Assert
        await Expect(Page.Locator("#displayText")).ToContainTextAsync(inputValue);
    }
}