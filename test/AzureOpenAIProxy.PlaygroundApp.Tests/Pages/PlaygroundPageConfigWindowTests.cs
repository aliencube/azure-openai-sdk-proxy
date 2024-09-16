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
                                     .Locator("fluent-tab#system-message-tab")
                                     .Locator("fluent-tab-panel#system-message-tab-panel");
        var parameterPanel = Page.Locator("div.config-grid")
                                 .Locator("div#config-window")
                                 .Locator($"div#{id}")
                                 .Locator("fluent-tabs#config-tabs")
                                 .Locator("fluent-tab#parameters-message-tab")
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
}
