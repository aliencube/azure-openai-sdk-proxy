using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class AdminEventsPageTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        IgnoreHTTPSErrors = true,
    };

    [Test]
    public async Task Given_Events_Page_When_Navigated_Then_It_Should_Have_ListEventDetailsComponent()
    {
        // Arrange
        await this.Page.GotoAsync("https://localhost:5001/events");

        // Act
        var listEvtDetailsComp = await Page.QuerySelectorAsync("#evt-detail-comp");

        // Assert
        Assert.That(listEvtDetailsComp, Is.Not.Null);
    }

    [Test]
    public async Task Given_Events_Page_When_Navigated_Then_It_Should_Have_EventDetailsTable()
    {
        // Arrange
        await this.Page.GotoAsync("https://localhost:5001/events");

        // wait for construct table
        await Task.Delay(2000);

        // Act
        var evtDetailTbl = await Page.QuerySelectorAsync("#evt-detail-tbl");

        // Assert
        Assert.That(evtDetailTbl, Is.Not.Null);
    }
}
