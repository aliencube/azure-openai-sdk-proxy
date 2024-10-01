using System;

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

    [Test]
    public async Task Given_Input_Event_Timezone_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        await Page.WaitForSelectorAsync("#event-timezone");

        string timeZone = await Page.EvaluateAsync<string>(@"() => Intl.DateTimeFormat().resolvedOptions().timeZone");

        // Act
        string inputTimezoneValue = await Page.GetAttributeAsync("#event-timezone", "current-value");

        // Assert
        inputTimezoneValue.Should().Be(timeZone);
    }

    [Test]
    public async Task Given_Input_Event_Start_DateTime_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        await Page.WaitForSelectorAsync("#event-start-date");
        await Page.WaitForSelectorAsync("#event-start-time");

        string timezoneId = await Page.EvaluateAsync<string>(@"() => Intl.DateTimeFormat().resolvedOptions().timeZone");
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        DateTimeOffset currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        var startTime = currentTime.AddHours(1).AddMinutes(-currentTime.Minute);


        // Act
        var inputStartDateValue = await Page.GetAttributeAsync("#event-start-date", "current-value");
        var inputStartTimeValue = await Page.GetAttributeAsync("#event-start-time", "current-value");

        // Assert
        inputStartDateValue.Should().Be(startTime.ToString("yyyy-MM-dd"));
        inputStartTimeValue.Should().Be(startTime.ToString("HH:mm"));
    }

    [Test]
    public async Task Given_Input_Event_End_DateTime_When_Initialized_Timezone_Then_It_Should_Update_Value()
    {
        // Arrange
        await Page.WaitForSelectorAsync("#event-end-date");
        await Page.WaitForSelectorAsync("#event-end-time");

        string timezoneId = await Page.EvaluateAsync<string>(@"() => Intl.DateTimeFormat().resolvedOptions().timeZone");
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        DateTimeOffset currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        var endTime = currentTime.AddDays(1).AddHours(1).AddMinutes(-currentTime.Minute);


        // Act
        var inputEndDateValue = await Page.GetAttributeAsync("#event-end-date", "current-value");
        var inputEndTimeValue = await Page.GetAttributeAsync("#event-end-time", "current-value");

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
