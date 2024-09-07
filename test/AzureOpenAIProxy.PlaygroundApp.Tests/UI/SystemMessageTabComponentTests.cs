using FluentAssertions;

using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class SystemMessageTabComponentTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        IgnoreHTTPSErrors = true,
    };

    [SetUp]
    public async Task Setup()
    {
        // Arrange
        await Page.GotoAsync("https://localhost:5001/tests");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    // label 보이는지 테스트(TestsPageTests.cs에 이미 있음)
    
    // 버튼 보이는지 테스트
    [Test]
    [TestCase("system-message-tab-apply-button")]
    [TestCase("system-message-tab-reset-button")]
    public async Task Given_Button_When_Page_Loaded_Then_Button_Should_Be_Visible(string id)
    {
        // Act
        var button = Page.Locator($"fluent-button#{id}");

        // Assert
        await Expect(button).ToBeVisibleAsync();
    }

    // 버튼 비활성화 여부 테스트(초기 로드) - 'disabled' 속성이 있으면 비활성화된 상태
    [Test]
    [TestCase("system-message-tab-apply-button")]
    [TestCase("system-message-tab-reset-button")]
    public async Task Given_Button_When_Page_Loaded_Then_The_Button_Should_Be_Disabled(string id)
    {
        // Arrange
        var button = Page.Locator($"fluent-button#{id}");

        // Act
        var isButtonDisabled = await button.GetAttributeAsync("disabled");

        // Assert
        isButtonDisabled.Should().NotBeNull();
    }

    // textarea 보이는지 테스트
    [Test]
    [TestCase("system-message-tab-textarea")]
    public async Task Given_TextArea_When_Page_Loaded_Then_TextArea_Should_Be_Visible(string id)
    {
        // Act
        var textarea = Page.Locator($"fluent-text-area#{id}");

        // Assert
        await Expect(textarea).ToBeVisibleAsync();
    }

    // textarea 초기 로드 시 default 값으로 설정되어 있는지 테스트
    [Test]
    [TestCase("debug-button-system-message-tab", "You are an AI assistant that helps people find information.", typeof(string))]
    public async Task Given_TextArea_When_Page_Loaded_Then_TextArea_Should_Have_Default_Value(string buttonId, string expectedValue, Type expectedType)
    {
        // Arrange 
        var debugButton = Page.Locator($"fluent-button#{buttonId}");

        // Act
        await debugButton.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{expectedValue} (Type: {expectedType})");
    }

    // textarea 값 변경 감지되면 apply 버튼 & reset 버튼 활성화 되는지 테스트
    /// -> 'disabled' 속성이 없으면 활성화된 상태
    [Test]
    public async Task Given_ApplyButton_And_ResetButton_When_TextArea_Value_Changed_Then_All_Buttons_Should_Be_Enabled()
    {
        // Arrange
        var applyButton = Page.Locator($"fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");
        var newValue = "New system message";

        // Act
        await Page.EvaluateAsync(@$"
                const textarea = document.querySelector('fluent-text-area#system-message-tab-textarea');
                textarea.value = '{newValue}';
                textarea.dispatchEvent(new Event('input', {{ bubbles: true }}));
        ");
        var isApplyButtonDisabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonDisabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        Assert.Multiple(() =>
        {
            isApplyButtonDisabled.Should().BeNull();
            isResetButtonDisabled.Should().BeNull();
        });
    }

    // apply 버튼 작동 테스트(변경된 textarea 값 system message에 적용 & apply 버튼 비활성화 + default 값과 다른 경우 reset 버튼 그대로 활성화)
    [Test]
    [TestCase("1 New system message 1", typeof(string))]
    [TestCase("2 New system message 2", typeof(string))]
    [TestCase("You are an AI assistant that helps people find information.", typeof(string))]
    public async Task Given_ApplyButton_When_Clicked_Then_TextArea_Value_Should_Be_Applied_As_SystemMessage_And_ApplyButton_Should_Be_Disabled(string expectedValue, Type expectedType)
    {
        // Arrange
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");
        var debugButton = Page.Locator($"fluent-button#debug-button-system-message-tab");
        var defaultValue = "You are an AI assistant that helps people find information.";

        // Act
        await Page.EvaluateAsync(@$"
                const textarea = document.querySelector('fluent-text-area#system-message-tab-textarea');
                textarea.value = '{expectedValue}';
                textarea.dispatchEvent(new Event('input', {{ bubbles: true }}));
        ");
        await applyButton.ClickAsync();
        await debugButton.ClickAsync();
        var isApplyButtonDisabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonDisabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        Assert.Multiple(async () =>
        {
            await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{expectedValue} (Type: {expectedType})");
            isApplyButtonDisabled.Should().NotBeNull();
            if (expectedValue == defaultValue)
            {
                isResetButtonDisabled.Should().NotBeNull();
            }
            else
            {
                isResetButtonDisabled.Should().BeNull();
            }
        });
    }

    // reset 버튼 작동 테스트(textarea 및 system message 값 초기화 & apply, reset 버튼 모두 비활성화)
    [Test]
    [TestCase("You are an AI assistant that helps people find information.", typeof(string))]
    public async Task Given_ResetButton_When_Clicked_Then_SystemMessage_And_TextArea_Should_Have_Default_Value_And_All_Buttons_Should_Be_Disabled(string expectedValue, Type expectedType)
    {
        // Arrange
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");
        var debugButton = Page.Locator($"fluent-button#debug-button-system-message-tab");
        var newValue = "New system message";

        // Act
        await Page.EvaluateAsync(@$"
                const textarea = document.querySelector('fluent-text-area#system-message-tab-textarea');
                textarea.value = '{newValue}';
                textarea.dispatchEvent(new Event('input', {{ bubbles: true }}));
        ");
        await applyButton.ClickAsync();
        await resetButton.ClickAsync();
        await debugButton.ClickAsync();
        var isApplyButtonDisabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonDisabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        Assert.Multiple(async () =>
        {
            await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{expectedValue} (Type: {expectedType})");
            isApplyButtonDisabled.Should().NotBeNull();
            isResetButtonDisabled.Should().NotBeNull();
        });
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}