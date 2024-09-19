using FluentAssertions;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[TestFixture]
[Property("Category", "Integration")]
public partial class PlaygroundPageTests
{
    [Test]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_Header_Should_Be_Displayed()
    {
        // Act
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator("h2");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [Test]
    [TestCase("api-key", "API key")]
    public async Task Given_ApiKeynputField_When_Endpoint_Invoked_Then_Label_Should_Be_Visible(string id, string label)
    {
        // Arrange
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator($"div#{id}")
                          .Locator("label[for='api-key-field']");

        // Act
        var result = await element.TextContentAsync();

        // Assert
        result.Should().StartWith(label);
    }

    [Test]
    [TestCase("api-key")]
    public async Task Given_ApiKeynputField_When_Endpoint_Invoked_Then_It_Should_Be_Visible(string id)
    {
        // Act
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator($"div#{id}")
                          .Locator("fluent-text-field#api-key-field");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [Test]
    [TestCase("api-key")]
    public async Task Given_ApiKeyInputField_When_Endpoint_Invoked_Then_It_Should_Be_Password_Type(string id)
    {
        // Arrange
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator($"div#{id}")
                          .Locator("fluent-text-field#api-key-field")
                          .Locator("input");

        // Act
        var inputType = await element.GetAttributeAsync("type");

        // Assert
        inputType.Should().Be("password");
    }

    [Test]
    [TestCase("api-key", "test-api-key-1")]
    [TestCase("api-key", "example-key-123")]
    public async Task Given_ApiKeyInputField_When_Changed_Then_It_Should_Be_Updated(string id, string apiKey)
    {
        // Arrange
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator($"div#{id}")
                          .Locator("fluent-text-field#api-key-field")
                          .Locator("input");

        // Act
        await element.FillAsync(apiKey);

        // Assert
        await Expect(element).ToHaveValueAsync(apiKey);
    }

    [Test]
    [TestCase("deployment-model-list", "Deployment")]
    public async Task Given_Label_When_Page_Loaded_Then_Label_Should_Be_Visible(string id, string label)
    {
        // Arrange
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator($"div#{id}")
                          .Locator("label[for='deployment-model-list-options']");

        // Act
        var result = await element.TextContentAsync();

        // Assert
        result.Should().StartWith(label);
    }

    [Test]
    [TestCase("deployment-model-list")]
    public async Task Given_DropdownList_When_Page_Loaded_Then_DropdownList_Should_Be_Visible(string id)
    {
        // Act
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator($"div#{id}")
                          .Locator("fluent-select#deployment-model-list-options");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [Test]
    [TestCase("deployment-model-list")]
    public async Task Given_DropdownList_When_DropdownList_Clicked_And_DropdownOptions_Appeared_Then_All_DropdownOptions_Should_Be_Visible(string id)
    {
        // Arrange
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator($"div#{id}")
                          .Locator("fluent-select#deployment-model-list-options");

        // Act
        await element.ClickAsync();
        var options = element.Locator("fluent-option");

        // Assert
        await Expect(options.Nth(0)).Not.ToBeVisibleAsync();
        for (int i = 1; i < await options.CountAsync(); i++)
        {
            await Expect(options.Nth(i)).ToBeVisibleAsync();
        }
    }

    [Test]
    [TestCase("config-tab")]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_ConfigTab_Should_Be_Displayed(string id)
    {
        // Act
        var element = Page.Locator("div.config-grid")
                          .Locator("div#config-window")
                          .Locator($"div#{id}")
                          .Locator("fluent-tabs#config-tabs");

        // Assert
        await Expect(element).ToBeVisibleAsync();
    }

    [Test]
    [TestCase("config-tab")]
    public async Task Given_ConfigTab_When_Endpoint_Invoked_Then_Id_Should_Be_System_Message_Tab(string id)
    {
        // Act
        var systemMessagePanel = Page.Locator("div.config-grid")
                                     .Locator("div#config-window")
                                     .Locator($"div#{id}")
                                     .Locator("fluent-tabs#config-tabs")
                                     .Locator("fluent-tab-panel#system-message-tab-panel");
        var parameterPanel = Page.Locator("div.config-grid")
                                 .Locator("div#config-window")
                                 .Locator($"div#{id}")
                                 .Locator("fluent-tabs#config-tabs")
                                 .Locator("fluent-tab-panel#parameters-tab-panel");

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
        var selectedTab = Page.Locator("div.config-grid")
                              .Locator("fluent-tabs#config-tabs")
                              .Locator(selectedTabSelector);
        var selectedPanel = Page.Locator("div.config-grid")
                                .Locator("fluent-tabs#config-tabs")
                                .Locator(selectedPanelSelector);
        var hiddenPanel = Page.Locator("div.config-grid")
                              .Locator("fluent-tabs#config-tabs")
                              .Locator(hiddenPanelSelector);

        // Act
        await selectedTab.ClickAsync();

        // Assert
        await Expect(selectedPanel).ToBeVisibleAsync();
        await Expect(hiddenPanel).ToBeHiddenAsync();
    }

    [Test]
    public async Task Given_SystemMessageTab_Buttons_When_TextArea_Value_Changed_Then_All_Buttons_Should_Be_Enabled()
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var systemMessageTextAreaControl = Page.Locator("fluent-text-area#system-message-tab-textarea textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await systemMessageTab.ClickAsync();
        await systemMessageTextAreaControl.FillAsync("New system message");
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        isApplyButtonEnabled.Should().BeNull();
        isResetButtonEnabled.Should().BeNull();
    }

    [Test]
    [TestCase("1 New system message 1")]
    [TestCase("2 New system message 2")]
    public async Task Given_SystemMessageTab_ApplyButton_When_Clicked_Then_Changed_TextArea_Value_Should_Be_Applied_As_SystemMessage_And_ApplyButton_Should_Be_Disabled_And_ResetButton_Should_Be_Enabled(
        string expectedValue
    )
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var systemMessageTextArea = Page.Locator("fluent-text-area#system-message-tab-textarea");
        var systemMessageTextAreaControl = Page.Locator("fluent-text-area#system-message-tab-textarea textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await systemMessageTab.ClickAsync();
        await systemMessageTextAreaControl.FillAsync(expectedValue);
        await applyButton.ClickAsync();
        var actualValue = await systemMessageTextArea.GetAttributeAsync("value");
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().NotBeNull();
        isResetButtonEnabled.Should().BeNull();
    }

    [Test]
    [TestCase("You are an AI assistant that helps people find information.")]
    public async Task Given_SystemMessageTab_ApplyButton_When_Clicked_Then_Default_TextArea_Value_Should_Be_Applied_As_SystemMessage_And_All_Buttons_Should_Be_Disabled(
        string expectedValue
    )
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var systemMessageTextArea = Page.Locator("fluent-text-area#system-message-tab-textarea");
        var systemMessageTextAreaControl = Page.Locator("fluent-text-area#system-message-tab-textarea textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await systemMessageTab.ClickAsync();
        await systemMessageTextAreaControl.FillAsync(expectedValue);
        await applyButton.ClickAsync();
        var actualValue = await systemMessageTextArea.GetAttributeAsync("value");
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().NotBeNull();
        isResetButtonEnabled.Should().NotBeNull();
    }

    [Test]
    [TestCase("You are an AI assistant that helps people find information.")]
    public async Task Given_SystemMessageTab_ResetButton_When_Clicked_Then_SystemMessage_And_TextArea_Should_Have_Default_Value_And_All_Buttons_Should_Be_Disabled(
        string expectedValue
    )
    {
        // Arrange
        var systemMessageTab = Page.Locator("fluent-tab#system-message-tab");
        var systemMessageTextArea = Page.Locator("fluent-text-area#system-message-tab-textarea");
        var systemMessageTextAreaControl = Page.Locator("fluent-text-area#system-message-tab-textarea textarea");
        var applyButton = Page.Locator("fluent-button#system-message-tab-apply-button");
        var resetButton = Page.Locator("fluent-button#system-message-tab-reset-button");

        // Act
        await systemMessageTab.ClickAsync();
        await systemMessageTextAreaControl.FillAsync("New system message");
        await applyButton.ClickAsync();
        await resetButton.ClickAsync();
        var actualValue = await systemMessageTextArea.GetAttributeAsync("value");
        var isApplyButtonEnabled = await applyButton.GetAttributeAsync("disabled");
        var isResetButtonEnabled = await resetButton.GetAttributeAsync("disabled");

        // Assert
        actualValue.Should().Be(expectedValue);
        isApplyButtonEnabled.Should().NotBeNull();
        isResetButtonEnabled.Should().NotBeNull();
    }
}
