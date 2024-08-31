using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;

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
            // API 키 입력 필드가 로드될 때까지 대기
            await Page.WaitForSelectorAsync("fluent-text-field");
        }

        [Test]
        public async Task Field_IsMasked_ByDefault()
        {
            // Arrange: API 키 입력 필드가 존재
            var apiKeyInput = Page.Locator("fluent-text-field >> input");

            // Act: API 키 입력 필드의 타입을 가져옴
            var inputType = await apiKeyInput.GetAttributeAsync("type");

            // Assert: API 키 입력 필드는 기본적으로 마스킹되어 있어야 함
            inputType.Should().Be("password", "API 키 입력 필드는 기본적으로 마스킹되어 있어야 합니다.");
        }

        [Test]
        [TestCase("test-api-key-1")]
        [TestCase("example-key-123")]
        public async Task Field_Displays_Value_After_Input(string apiKey)
        {
            // Arrange: API 키 입력 필드가 존재
            var apiKeyInput = Page.Locator("fluent-text-field >> input");

            // Act: API 키 값을 입력했을 때
            await apiKeyInput.FillAsync(apiKey);

            // Assert: API 키 입력 필드에 입력한 값이 표시되어야 함
            await Expect(apiKeyInput).ToHaveValueAsync(apiKey);
        }

        [TearDown]
        public async Task CleanUp()
        {
            await Page.CloseAsync();
        }
    }
}
