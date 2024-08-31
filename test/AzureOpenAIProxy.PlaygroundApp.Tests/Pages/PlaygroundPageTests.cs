using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using FluentAssertions;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class PlaygroundPageTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new() { IgnoreHTTPSErrors = true, };

    [SetUp]
    public async Task SetUp()
    {
        await Page.GotoAsync("https://localhost:5001/playground/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Test]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_ConfigTab_Should_Be_Displayed()
    {
        // Act
        var configTab = Page.Locator("fluent-tabs#config-tab");

        // Assert
        await Expect(configTab).ToBeVisibleAsync();
    }

    [Test]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_Id_Should_Be_System_Message_Tab()
    {
        // Act
        var systemMessagePanel = Page.Locator("fluent-tab-panel#system-message-tab-panel");
        var parameterPanel = Page.Locator("fluent-tab-panel#parameters-tab-panel");

        // Assert
        await Expect(systemMessagePanel).ToBeVisibleAsync();
        await Expect(parameterPanel).ToBeHiddenAsync();
    }

    [Test]
    [TestCase(
        "fluent-tab#parameters-tab",
        "fluent-tab-panel#parameters-tab-panel",
        "fluent-tab-panel#system-message-tab-panel"
    )]
    [TestCase(
        "fluent-tab#system-message-tab",
        "fluent-tab-panel#system-message-tab-panel",
        "fluent-tab-panel#parameters-tab-panel"
    )]
    public async Task Given_ConfigTab_When_Changed_Then_Tab_Should_Be_Updated(
        string selectedTabSelector,
        string selectedPanelSelector,
        string hiddenPanelSelector
    )
    {
        // Arrange
        var selectedTab = Page.Locator(selectedTabSelector);
        var selectedPanel = Page.Locator(selectedPanelSelector);
        var hiddenPanel = Page.Locator(hiddenPanelSelector);

        // Act
        await selectedTab.ClickAsync();

        // Assert
        await Expect(selectedPanel).ToBeVisibleAsync();
        await Expect(hiddenPanel).ToBeHiddenAsync();
    }

   [Test]
    public async Task Given_ApiKeyInputField_When_FieldIsLoaded_Then_FieldIsMaskedByDefault()
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field >> input");

        // Act
        var inputType = await apiKeyInput.GetAttributeAsync("type");

        // Assert
        inputType.Should().NotBeNull("API 키 입력 필드의 타입을 가져오는 데 실패했습니다.");
        inputType.Should().Be("password", "API 키 입력 필드는 기본적으로 마스킹되어 있어야 합니다.");
    }

    [Test]
    [TestCase("test-api-key-1")]
    [TestCase("example-key-123")]
    public async Task Given_ApiKeyInputField_When_ValuesAreEntered_Then_FieldDisplaysInputValue(string apiKey)
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field >> input");

        // Act
        await apiKeyInput.FillAsync(apiKey);

        // Assert
        await Expect(apiKeyInput).ToHaveValueAsync(apiKey);
    }

    [Test]
    public async Task Given_ApiKeyInputField_When_ValueIsSet_Then_CurrentValueShouldBeCorrect()
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field >> input");
        var apiKey = "test-api-key";

        // Act
        await apiKeyInput.FillAsync(apiKey);
        var currentValue = await apiKeyInput.InputValueAsync();

        // Assert
        currentValue.Should().Be(apiKey, "API 키 입력 필드에 값이 제대로 설정되지 않았습니다.");
    }

}