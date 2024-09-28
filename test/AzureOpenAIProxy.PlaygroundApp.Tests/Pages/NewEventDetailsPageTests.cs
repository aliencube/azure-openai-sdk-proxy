using FluentAssertions;

using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.VisualBasic;

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
        var inputTitle = await Page.QuerySelectorAsync("#event-title");
        var inputSummary = await Page.QuerySelectorAsync("#event-summary");
        var inputDescription = await Page.QuerySelectorAsync("#event-description");
        var inputStartDate = await Page.QuerySelectorAsync("#event-start-date");
        var inputStartTime = await Page.QuerySelectorAsync("#event-start-time");
        var inputEndDate = await Page.QuerySelectorAsync("#event-end-date");
        var inputEndTime = await Page.QuerySelectorAsync("#event-end-time");
        var inputTimezone = await Page.QuerySelectorAsync("#event-timezone");
        var inputOrganizerName = await Page.QuerySelectorAsync("#event-organizer-name");
        var inputOrganizerEmail = await Page.QuerySelectorAsync("#event-organizer-email");
        var inputCoorgnizerName = await Page.QuerySelectorAsync("#event-coorgnizer-name");
        var inputCoorgnizerEmail = await Page.QuerySelectorAsync("#event-coorgnizer-email");
        var inputMaxTokenCap = await Page.QuerySelectorAsync("#event-max-token-cap");
        var inputDailyRequestCap = await Page.QuerySelectorAsync("#event-daily-request-cap");
        var buttonAdd = await Page.QuerySelectorAsync("#admin-event-detail-add");
        var buttonCancel = await Page.QuerySelectorAsync("#admin-event-detail-cancel");

        // Assert
        inputTitle.Should().NotBeNull();
        inputSummary.Should().NotBeNull();
        inputDescription.Should().NotBeNull();
        inputStartDate.Should().NotBeNull();
        inputStartTime.Should().NotBeNull();
        inputEndDate.Should().NotBeNull();
        inputEndTime.Should().NotBeNull();
        inputTimezone.Should().NotBeNull();
        inputOrganizerName.Should().NotBeNull();
        inputOrganizerEmail.Should().NotBeNull();
        inputCoorgnizerName.Should().NotBeNull();
        inputCoorgnizerEmail.Should().NotBeNull();
        inputMaxTokenCap.Should().NotBeNull();
        inputDailyRequestCap.Should().NotBeNull();
        buttonAdd.Should().NotBeNull();
        buttonCancel.Should().NotBeNull();
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}
