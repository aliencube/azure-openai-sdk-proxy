using FluentAssertions;

using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]

public class NewEventDetailsPageTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        IgnoreHTTPSErrors = true,
    };

    [SetUp]
    public async Task Init()
    {
        await Page.GotoAsync("https://localhost:5001/admin/events/new");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    public async Task Given_New_Event_Details_Page_When_Navigated_Then_It_Should_Load_Correctly()
    {
        // Act
        var header = await Page.QuerySelectorAsync("fluent-header");
        var addButton = await Page.QuerySelectorAsync("#admin-event-detail-add");
        var cancelButton = await Page.QuerySelectorAsync("#admin-event-detail-cancel");

        // Assert
        header.Should().NotBeNull();
        addButton.Should().NotBeNull();
        cancelButton.Should().NotBeNull();
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}
