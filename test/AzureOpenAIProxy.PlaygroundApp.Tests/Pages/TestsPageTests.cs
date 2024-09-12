using FluentAssertions;

using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[Property("Category", "Integration")]
public class TestsPageTests : PageTest
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
    public async Task Given_No_Input_On_DebugTarget_When_DebugButton_Clicked_Then_Toast_Should_Show_NullMessage()
    {
        // Arrange
        var button = Page.Locator("fluent-button#debug-button");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync("Input is null.");
    }

    [Test]
    [TestCase(123, typeof(int))]
    [TestCase(456, typeof(int))]
    [TestCase(789, typeof(int))]
    public async Task Given_Input_On_DebugTarget_When_DebugButton_Clicked_Then_Toast_Should_Show_Input(int inputValue, Type expectedType)
    {
        // Arrange
        var radio = Page.Locator("fluent-radio-group#debug-target")
                        .Locator($"fluent-radio[current-value='{inputValue}']");
        var button = Page.Locator("fluent-button#debug-button");

        // Act
        await radio.ClickAsync();
        await button.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{inputValue} (Type: {expectedType})");
    }

    [Test]
    [TestCase("deployment-model-label", "Deployment")]
    public async Task Given_Label_When_Page_Loaded_Then_Label_Should_Be_Visible(string id, string expected)
    {
        // Arrange
        var label = Page.Locator($"label#{id}");

        // Act
        var result = await label.TextContentAsync();

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    [TestCase("deployment-model-list-options")]
    public async Task Given_DropdownList_When_Page_Loaded_Then_DropdownList_Should_Be_Visible(string id)
    {
        // Act
        var select = Page.Locator($"fluent-select#{id}");

        // Assert
        await Expect(select).ToBeVisibleAsync();
    }

    [Test]
    [TestCase("deployment-model-list-options")]
    public async Task Given_DropdownList_When_DropdownList_Clicked_And_DropdownOptions_Appeared_Then_All_DropdownOptions_Should_Be_Visible(string id)
    {
        // Arrange
        var fluentSelect = Page.Locator($"fluent-select#{id}");

        // Act
        await fluentSelect.ClickAsync();
        var fluentOptions = fluentSelect.Locator("fluent-option");

        // Assert
        for (int i = 0; i < await fluentOptions.CountAsync(); i++)
        {
            await Expect(fluentOptions.Nth(i)).ToBeVisibleAsync();
        }
    }

    [Test]
    [TestCase(2, "AZ", typeof(string))]
    [TestCase(4, "CA", typeof(string))]
    [TestCase(6, "CT", typeof(string))]
    [TestCase(8, "FL", typeof(string))]
    public async Task Given_DropdownOptions_And_ExpectedValue_When_Third_DropdownOption_Selected_And_DropdownValue_Updated_Then_DropdownValue_Should_Match_ExpectedValue(int index, string expectedValue, Type expectedType)
    {
        // Arrange
        var fluentSelect = Page.Locator("fluent-select#deployment-model-list-options");
        await fluentSelect.ClickAsync();
        var fluentOptions = fluentSelect.Locator("fluent-option");
        var button = Page.Locator("fluent-button#debug-button-deployment-model-list");

        // Act
        await fluentOptions.Nth(index).ScrollIntoViewIfNeededAsync();
        await fluentOptions.Nth(index).ClickAsync();
        await button.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{expectedValue} (Type: {expectedType})");
    }
    
    [Test]
    public async Task Given_Null_ApiKeyInput_When_DebugButton_Clicked_Then_Should_Show_NullMessage()
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field#api-key-input").Locator("input");
        var debugButton = Page.Locator("fluent-button#debug-api-key");

        // Act
        await apiKeyInput.FillAsync("");
        await debugButton.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync("Input is null.");
    }


    [Test]
    [TestCase("test-api-key-321")]
    [TestCase("example-key-123")]
    public async Task Given_ApiKeyInput_When_Input_Entered_Then_It_Should_Show_Input(string apiKey)
    {
        // Arrange
        var apiKeyInput = Page.Locator("fluent-text-field#api-key-input").Locator("input");
        var debugButton = Page.Locator("fluent-button#debug-api-key");

        // Act
        await apiKeyInput.FillAsync(apiKey);
        await debugButton.ClickAsync();

        // Assert
        await Expect(Page.Locator(".fluent-toast-title")).ToHaveTextAsync($"{apiKey} (Type: System.String)");
    }
    
    [TearDown]
    public async Task CleanUp()
    {
        await Page.CloseAsync();
    }

    
}