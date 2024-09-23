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
        var eventListComponent = await Page.QuerySelectorAsync("#event-list");

        // Assert
        eventListComponent.Should().NotBeNull();
    }

    [Test]
    public async Task Given_Events_When_Loaded_Then_It_Should_Have_Less_Than_Or_Equal_To_Four_EventItemComponents()
    {
        // Act
        var availableEvents = await Page.QuerySelectorAsync("#event-list");

        var childrenCount = await availableEvents.EvaluateAsync<int>("elist => elist.children.length");

        // Assert
        Assert.That(childrenCount, Is.GreaterThan(0));
        Assert.That(childrenCount, Is.LessThanOrEqualTo(4));
    }

    [Test]
    public async Task Given_Events_When_Loaded_Then_It_Should_Have_Header_And_Summary_In_The_Card()
    {
        // Act
        var eventCards = await Page.Locator("div.fluent-card-minimal-style.event").AllAsync();

        // Assert
        foreach (var card in eventCards)
        {
            card.Should().NotBeNull();
            // Check headers
            var header = card.Locator("div.fluent-nav-item.event-details-link").First;
            header.Should().NotBeNull();

            // Check summaries
            var summary = card.Locator("div.event-summary.card.border").First;
            summary.Should().NotBeNull();
        }
    }

    [Test]
    public async Task Given_Events_When_Loaded_Then_Their_Links_Are_Enabled_To_Click()
    {
        // Act
        var eventCards = await Page.Locator("div.fluent-card-minimal-style.event").AllAsync();

        // Assert
        foreach (var card in eventCards)
        {
            // Getting a link element.
            var link = card.Locator("div.fluent-nav-item.event-details-link").First
                .Locator("a.fluent-nav-link").First;

            link.Should().NotBeNull();
            await Expect(link).ToBeEnabledAsync();
        }
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}