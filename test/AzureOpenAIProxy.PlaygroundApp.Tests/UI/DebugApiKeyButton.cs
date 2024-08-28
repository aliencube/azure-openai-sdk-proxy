using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace AzureOpenAIProxy.PlaygroundApp.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ApiKeyMaskingTests : PageTest
    {
        public override BrowserNewContextOptions ContextOptions() => new()
        {
            IgnoreHTTPSErrors = true,
        };

        [SetUp]
        public async Task SetUp()
        {
            await Page.GotoAsync("https://localhost:5001/");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        [Test]
        public async Task DefaultMaskedState()
        {
            var apiKeyInput = Page.Locator("input[name='ApiKey']");
            var inputType = await apiKeyInput.GetAttributeAsync("type");
            NUnit.Framework.Assert.AreEqual("password", inputType, "API 키 입력 필드는 기본적으로 마스킹되어 있어야 합니다.");
        }

        [Test]
        public async Task ToggleMasking()
        {
            var apiKeyInput = Page.Locator("input[name='ApiKey']");
            var toggleMaskButton = Page.GetByRole(AriaRole.Button, new() { Name = "Mask" }); 
            await Expect(apiKeyInput).ToHaveAttributeAsync("type", "password");
            await toggleMaskButton.ClickAsync();
            await Expect(apiKeyInput).ToHaveAttributeAsync("type", "text");

            await toggleMaskButton.ClickAsync();
            await Expect(apiKeyInput).ToHaveAttributeAsync("type", "password");
        }

        [Test]
        [TestCase("test-api-key-1")]
        [TestCase("example-key-123")]
        public async Task DisplayApiKeyInput(string apiKey)
        {
            var apiKeyInput = Page.Locator("input[name='ApiKey']");

            await apiKeyInput.FillAsync(apiKey);
            await Expect(apiKeyInput).ToHaveValueAsync(apiKey);
        }

        [TearDown]
        public async Task CleanUp()
        {
            await Page.CloseAsync();
        }
    }
}