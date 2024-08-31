using FluentAssertions;

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
        await this.Page.GotoAsync("https://localhost:5001/admin/events");

        // Act
        var adminEventsComponent = await Page.QuerySelectorAsync("#admin-events-component");

        // Assert
        adminEventsComponent.Should().NotBeNull();
    }

    [Test]
    public async Task Given_Events_Page_When_Navigated_Then_It_Should_Have_EventDetailsTable()
    {
        // Arrange
        await this.Page.GotoAsync("https://localhost:5001/admin/events");

        // wait for construct table
        await Task.Delay(2000);

        // Act
        var adminEventsTable = await Page.QuerySelectorAsync("#admin-events-table");

        // Assert
        adminEventsTable.Should().NotBeNull();
    }
}
