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
    public async Task Given_No_Input_When_DebugButton_Clicked_Then_Toast_Should_Show_NullMessage()
    {
        // Act
        await Page.GetByRole(AriaRole.Button, new() { Name = "Debug" }).ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync("Input is null.");
    }

    [Test]
    [TestCase(123, typeof(int))]
    [TestCase(456, typeof(int))]
    [TestCase(789, typeof(int))]
    public async Task Given_Input_When_DebugButton_Clicked_Then_Toast_Should_Show_Input(int inputValue, Type inputType)    {
        // Act
        await Page.GetByRole(AriaRole.Radio, new() { Name = $"{inputValue}" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Debug" }).ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{inputValue} (Type: {inputType})");
    }
}