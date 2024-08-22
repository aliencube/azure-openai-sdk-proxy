using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class HomePageTests : PageTest
{
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
