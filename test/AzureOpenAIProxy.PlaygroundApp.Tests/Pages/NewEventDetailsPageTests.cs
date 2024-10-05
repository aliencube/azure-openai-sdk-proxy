using FluentAssertions;

using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using TimeZoneConverter;

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
    [TestCase("admin-event-detail-add")]
    [TestCase("admin-event-detail-cancel")]
    public async Task Given_New_Event_Details_Page_When_Navigated_Then_It_Should_Load_Correctly(string id)
    {
        // Act
        var element = Page.Locator($"#{id}");

        var inputValue = await element.GetAttributeAsync("current-value");

        // Assert
        inputValue.Should().NotBeNull();
    }

    [Test]
    public async Task Given_Input_Event_Timezone_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        var inputTimezone = Page.Locator("#event-timezone");
        await inputTimezone.WaitForAsync();

        string timeZone = OperatingSystem.IsWindows() ? TZConvert.WindowsToIana(TimeZoneInfo.Local.Id) : TimeZoneInfo.Local.Id;

        // Act
        string inputTimezoneValue = await inputTimezone.GetAttributeAsync("current-value");

        // Assert
        inputTimezoneValue.Should().Be(timeZone);
    }

    [Test]
    public async Task Given_Input_Event_Start_DateTime_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        var inputStartDate = Page.Locator("#event-start-date");
        var inputStartTime = Page.Locator("#event-start-time");

        await inputStartDate.WaitForAsync();
        await inputStartTime.WaitForAsync();

        string timezoneId = OperatingSystem.IsWindows() ? TZConvert.WindowsToIana(TimeZoneInfo.Local.Id) : TimeZoneInfo.Local.Id;
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        DateTimeOffset currentTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZoneInfo);
        var startTime = currentTime.AddHours(1).AddMinutes(-currentTime.Minute);

        // Act
        var inputStartDateValue = await inputStartDate.GetAttributeAsync("current-value");
        var inputStartTimeValue = await inputStartTime.GetAttributeAsync("current-value");

        // Assert
        inputStartDateValue.Should().Be(startTime.ToString("yyyy-MM-dd"));
        inputStartTimeValue.Should().Be(startTime.ToString("HH:mm"));
    }

    [Test]
    public async Task Given_Input_Event_End_DateTime_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        var inputEndDate = Page.Locator("#event-end-date");
        var inputEndTime = Page.Locator("#event-end-time");

        await inputEndDate.WaitForAsync();
        await inputEndTime.WaitForAsync();

        string timezoneId = OperatingSystem.IsWindows() ? TZConvert.WindowsToIana(TimeZoneInfo.Local.Id) : TimeZoneInfo.Local.Id;
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        DateTimeOffset currentTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZoneInfo);
        var endTime = currentTime.AddDays(1).AddHours(1).AddMinutes(-currentTime.Minute);

        // Act
        var inputEndDateValue = await inputEndDate.GetAttributeAsync("current-value");
        var inputEndTimeValue = await inputEndTime.GetAttributeAsync("current-value");

        // Assert
        inputEndDateValue.Should().Be(endTime.ToString("yyyy-MM-dd"));
        inputEndTimeValue.Should().Be(endTime.ToString("HH:mm"));
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}
