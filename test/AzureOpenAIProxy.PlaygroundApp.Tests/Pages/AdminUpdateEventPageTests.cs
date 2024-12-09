using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class AdminUpdateEventPageTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new() { IgnoreHTTPSErrors = true, };

    [SetUp]
    public async Task Init()
    {
        var eventId = Guid.NewGuid();
        await Page.GotoAsync($"https://localhost:5001/admin/events/edit/{eventId}");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    [TestCase("event-title")]
    [TestCase("event-summary")]
    [TestCase("event-description")]
    [TestCase("event-start-date")]
    [TestCase("event-start-time")]
    [TestCase("event-end-date")]
    [TestCase("event-end-time")]
    [TestCase("event-timezone")]
    [TestCase("event-organizer-name")]
    [TestCase("event-organizer-email")]
    [TestCase("event-coorgnizer-name")]
    [TestCase("event-coorgnizer-email")]
    [TestCase("event-max-token-cap")]
    [TestCase("event-daily-request-cap")]
    [TestCase("admin-event-detail-update")]
    [TestCase("admin-event-detail-cancel")]
    public async Task Given_Update_Event_Details_Page_When_Navigated_Then_It_Should_Load_Correctly(string id)
    {
        // Act
        var element = Page.Locator($"#{id}");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}