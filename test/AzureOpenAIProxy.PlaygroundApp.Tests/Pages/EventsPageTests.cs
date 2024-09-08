using FluentAssertions;

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

    [SetUp]
    public async Task Init()
    {
        await Page.GotoAsync("https://localhost:5001/events");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    // Grid check
    [Test]
    public async Task Given_Events_Page_When_Navigated_Then_It_Should_Have_EventListComponent()
    {
        // Act
        var eventListComponent = await Page.QuerySelectorAsync("#events-list");

        // Assert
        eventListComponent.Should().NotBeNull();
    }

    [Test]
    public async Task Given_Events_When_Loaded_Then_It_Should_Have_Less_Than_Or_Equal_To_Four_EventItemComponents()
    {
        // Act
        var availableEvents = await Page.QuerySelectorAsync("#events-list");

        var childrenCount = await availableEvents.EvaluateAsync<int>("elist => elist.children.length");

        // Assert
        Assert.That(childrenCount, Is.GreaterThan(0));
        Assert.That(childrenCount, Is.LessThanOrEqualTo(4));
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}