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

    [SetUp]
    public async Task Init()
    {
        await Page.GotoAsync("https://localhost:5001/admin/events");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    public async Task Given_Events_Page_When_Navigated_Then_It_Should_Have_ListEventDetailsComponent()
    {
        // Act
        var adminEventsComponent = await Page.QuerySelectorAsync("#admin-events-component");

        // Assert
        adminEventsComponent.Should().NotBeNull();
    }

    [Test]
    public async Task Given_Events_Page_When_Navigated_Then_It_Should_Have_EventDetailsTable()
    {
        // wait for construct table
        await Task.Delay(2000);

        // Act
        var adminEventsTable = await Page.QuerySelectorAsync("#admin-events-table");

        // Assert
        adminEventsTable.Should().NotBeNull();
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}
