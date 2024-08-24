using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class EventsPageTests : PageTest
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
        Assert.IsNotNull(listEvtDetailsComp, "Test Failed: #evt-detail-comp not found");
    }

    [Test]
    public async Task Given_Events_Page_When_Navigated_Then_It_Should_Have_EventDetailsTable()
    {
        // Arrange
        await this.Page.GotoAsync("https://localhost:5001/events");

        // wait for construct table
        await Task.Delay(200);

        // Act
        var evtDetailTbl = await Page.QuerySelectorAsync("table.evt-detail-tbl");

        // Assert
        Assert.IsNotNull(evtDetailTbl, "Test Failed: table.evt-detail-tbl not found");
    }

    [Test]
    public async Task Given_Events_Page_When_Navigated_Then_It_Should_Have_EventDetailsHeader()
    {
        // Arrange
        await this.Page.GotoAsync("https://localhost:5001/events");

        // wait for construct table header
        await Task.Delay(200);

        // Act
        var evtDetailElemHeader = await Page.QuerySelectorAsync("div.evt-detail-header");

        // Assert
        Assert.IsNotNull(evtDetailElemHeader, "Test Failed: div.evt-detail-header not found");
    }
}
