using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class HomePageTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        IgnoreHTTPSErrors = true,
    };

    [SetUp]
    public async Task Setup()
    {
        await Page.GotoAsync("https://localhost:5001");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    public void Given_Root_Page_When_Navigated_Then_It_Should_No_Sidebar()
    {
        // Act
        var sidebar = Page.Locator("div.sidebar");

        // Assert
        Expect(sidebar).Equals(null);
    }
}
