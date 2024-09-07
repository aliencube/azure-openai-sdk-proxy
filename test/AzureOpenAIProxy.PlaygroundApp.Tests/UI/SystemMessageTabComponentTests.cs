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

    [Test]
    [TestCase("system-message-tab-label", "System message")]
    public async Task Given_Label_When_Page_Loaded_Then_Label_Should_Be_Visible(string id, string expected)
    {
        // Arrange
        var label = Page.Locator($"label#{id}");

        // Act
        var result = await label.TextContentAsync();

        // Assert
        result.Should().Be(expected);
    }
    
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

    // 버튼 비활성화 여부 테스트(초기 화면)
    [Test]
    [TestCase("system-message-tab-apply-button")]
    [TestCase("system-message-tab-reset-button")]
    public async Task Given_Button_When_Page_Loaded_Then_Button_Should_Be_Disabled(string id)
    {
        // Arrange
        var button = Page.Locator($"fluent-button#{id}");

        // Act
        var isButtonDisabled = await button.GetAttributeAsync("disabled");

        // Assert
        isButtonDisabled.Should().NotBeNull();
    }

    // 텍스트 박스 보이는지 테스트
    [Test]
    [TestCase("system-message-tab-textarea")]
    public async Task Given_TextArea_When_Page_Loaded_Then_TextArea_Should_Be_Visible(string id)
    {
        // Act
        var textarea = Page.Locator($"fluent-text-area#{id}");

        // Assert
        await Expect(textarea).ToBeVisibleAsync();
    }

    // 텍스트 박스 default 값 테스트
    [Test]
    [TestCase("debug-button-system-message-tab", "You are an AI assistant that helps people find information.", typeof(string))]
    public async Task Given_TextArea_When_Page_Loaded_Then_TextArea_Should_Have_Default_Value(string buttonId, string expectedValue, Type expectedType)
    {
        // Arrange 
        var debugButton = Page.Locator($"fluent-button#{buttonId}");

        // Act
        await debugButton.ClickAsync();
        // var actualValue = await textarea.TextContentAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{expectedValue} (Type: {expectedType})");
    }

    // system message textarea 값 변경했을 때 apply, reset 버튼 활성화 여부 테스트
    [Test]
    [TestCase("system-message-tab-apply-button")]
    [TestCase("system-message-tab-reset-button")]
    public async Task Given_TextArea_When_Value_Changed_Then_Buttons_Should_Be_Enabled(string id)
    {
        // Arrange
        // var textarea = Page.Locator("fluent-text-area#system-message-tab-textarea");
        var button = Page.Locator($"fluent-button#{id}");
        var newValue = "New system message";

        // Act
        await Page.EvaluateAsync($"document.querySelector('fluent-text-area#system-message-tab-textarea').value = '{newValue}'");

        // Assert
        var isButtonDisabled = await button.GetAttributeAsync("disabled");
        isButtonDisabled.Should().BeNullOrEmpty();
    }

    // apply 버튼 작동 테스트(textarea 값 적용 뒤 apply 버튼만 비활성화)
    [Test]
    [TestCase("New system message")]
    public async Task Given_ApplyButton_When_Clicked_Then_Changes_Should_Be_Applied_And_Buttons_Updated(string message)
    {
        // Arrange
        //var textarea = await Page.EvaluateAsync<IElementHandle>("document.querySelector('fluent-text-area#system-message-tab-textarea')");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var debugButton = Page.Locator($"fluent-button#debug-button-system-message-tab");
        var newValue = message;

        // Act
        await Page.EvaluateAsync($"document.querySelector('fluent-text-area#system-message-tab-textarea').value = '{newValue}'");
        await applyButton.ClickAsync();
        await debugButton.ClickAsync();
        var isApplyButtonDisabled = await applyButton.IsDisabledAsync();
        var actualValue = await Page.EvaluateAsync<string>("document.querySelector('fluent-text-area#system-message-tab-textarea').value");

        // Assert
        //isApplyButtonDisabled.Should().BeTrue();
        actualValue.Should().Be(newValue);
    }

    // reset 버튼 작동 테스트(textarea 값 초기화 뒤 apply, reset 버튼 모두 비활성화)
    [Test]
    public async Task Given_ResetButton_When_Clicked_Then_TextArea_Should_Have_Default_Value_And_Buttons_Updated()
    {
        // Arrange
        //var textarea = Page.Locator("fluent-text-area#system-message-tab-textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await resetButton.ClickAsync();
        var actualValue = await Page.EvaluateAsync<string>("document.querySelector('fluent-text-area#system-message-tab-textarea').value");

        // Assert
        actualValue.Should().Be("You are an AI assistant that helps people find information.");
        // var isApplyButtonDisabled = await applyButton.IsDisabledAsync();
        // var isResetButtonDisabled = await resetButton.IsDisabledAsync();
        // isApplyButtonDisabled.Should().BeTrue();
        // isResetButtonDisabled.Should().BeTrue();
    }

    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }
}