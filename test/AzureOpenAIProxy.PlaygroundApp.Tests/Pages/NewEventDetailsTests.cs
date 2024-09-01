using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class NewEventDetailsTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        IgnoreHTTPSErrors = true,
    };

    [Test]
    public async Task Given_Root_Page_When_Navigated_Then_It_Should_No_Sidebar()
    {
        // Arrange
        await this.Page.GotoAsync("https://localhost:5001");

        // Act
        var sidebar = this.Page.Locator("div.sidebar");

        // Assert
        Expect(sidebar).Equals(null);
    }
}
