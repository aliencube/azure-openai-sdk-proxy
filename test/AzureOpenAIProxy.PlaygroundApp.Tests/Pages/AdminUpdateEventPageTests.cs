using FluentAssertions;

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

 [Test]
    public async Task Given_Input_Event_Timezone_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        var inputTimezone = Page.Locator("#event-timezone");

        string timeZone = GetIanaTimezoneId();

        // Act
        string inputTimezoneValue = await inputTimezone.GetAttributeAsync("current-value");

        // Assert
        inputTimezoneValue.Should().Be(timeZone);
    }

    [Test]
    public async Task Given_Input_Event_Start_Date_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        var inputStartDate = Page.Locator("#event-start-date");

        string timezoneId = GetIanaTimezoneId();
        DateTimeOffset currentTime = GetCurrentDateTimeOffset(timezoneId);
        var startTime = currentTime.AddHours(1).AddMinutes(-currentTime.Minute);

        // Act
        var inputStartDateValue = await inputStartDate.GetAttributeAsync("current-value");

        // Assert
        inputStartDateValue.Should().Be(startTime.ToString("yyyy-MM-dd"));
    }

    [Test]
    public async Task Given_Input_Event_Start_Time_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        var inputStartTime = Page.Locator("#event-start-time");

        string timezoneId = GetIanaTimezoneId();
        DateTimeOffset currentTime = GetCurrentDateTimeOffset(timezoneId);
        var startTime = currentTime.AddHours(1).AddMinutes(-currentTime.Minute);

        // Act
        var inputStartTimeValue = await inputStartTime.GetAttributeAsync("current-value");

        // Assert
        inputStartTimeValue.Should().Be(startTime.ToString("HH:mm"));
    }

    [Test]
    public async Task Given_Input_Event_End_Date_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        var inputEndDate = Page.Locator("#event-end-date");

        string timezoneId = GetIanaTimezoneId();
        DateTimeOffset currentTime = GetCurrentDateTimeOffset(timezoneId);
        var endTime = currentTime.AddDays(1).AddHours(1).AddMinutes(-currentTime.Minute);

        // Act
        var inputEndDateValue = await inputEndDate.GetAttributeAsync("current-value");

        // Assert
        inputEndDateValue.Should().Be(endTime.ToString("yyyy-MM-dd"));
    }

    [Test]
    public async Task Given_Input_Event_End_Time_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        var inputEndTime = Page.Locator("#event-end-time");

        string timezoneId = GetIanaTimezoneId();
        DateTimeOffset currentTime = GetCurrentDateTimeOffset(timezoneId);
        var endTime = currentTime.AddDays(1).AddHours(1).AddMinutes(-currentTime.Minute);

        // Act
        var inputEndTimeValue = await inputEndTime.GetAttributeAsync("current-value");

        // Assert
        inputEndTimeValue.Should().Be(endTime.ToString("HH:mm"));
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }

    private string GetIanaTimezoneId()
    {
        string timezoneId = TimeZoneInfo.Local.Id;

        if (OperatingSystem.IsWindows())
        {
            if (TimeZoneInfo.TryConvertWindowsIdToIanaId(timezoneId, out var ianaTimezoneId))
            {
                timezoneId = ianaTimezoneId;
            }
        }

        return timezoneId;
    }

    private DateTimeOffset GetCurrentDateTimeOffset(string timezoneId)
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);

        return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZoneInfo);
    }
}