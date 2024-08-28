using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using Assert = NUnit.Framework.Assert;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.UI
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [Property("Category", "Integration")]
    public class ModelDropdownListComponentTests : PageTest
    {
        public override BrowserNewContextOptions ContextOptions() => new()
        {
            IgnoreHTTPSErrors = true,
        };

        [SetUp]
        public async Task Init()
        {
            await Page.GotoAsync("http://localhost:5000/tests");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        [Test]
        // 페이지에서 컴포넌트 레이블이 올바르게 표시되는지 확인
        public async Task Given_Label_When_Page_Loaded_Then_Label_Should_Be_Visible()
        {
            // given
            var label = Page.GetByText("Deployment");
            
            // when & then
            await Expect(label).ToBeVisibleAsync();
        }

        [Test]
        // 페이지에서 드롭다운 컴포넌트가 올바르게 표시되는지 확인
        public async Task Given_Dropdown_When_Page_Loaded_Then_Dropdown_Should_Be_Visible()
        {
            // given
            var fluentSelect = Page.Locator("fluent-select#deployment-model-list");

            // when & then
            await Expect(fluentSelect).ToBeVisibleAsync();
        }

        [Test]
        // 드롭다운의 옵션 값이 존재하는지 확인
        public async Task Given_Dropdown_When_Dropdown_Clicked_And_DropdownOptions_Appeared_Then_All_DropdownOptions_Should_Be_Visible()
        {
            // given
            var fluentSelect = Page.Locator("fluent-select#deployment-model-list");

            // when
            await fluentSelect.ClickAsync();
            var fluentOptions = fluentSelect.Locator("fluent-option");

            // then
            for (int i = 0; i < await fluentOptions.CountAsync(); i++)
            {
                await Expect(fluentOptions.Nth(i)).ToBeVisibleAsync();
            }
        }

        [Test]
        // 드롭다운의 옵션 값을 선택하면 부모 컴포넌트(페이지 컴포넌트)에 올바르게 업데이트 되는지 확인
        public async Task Given_DropdownOptions_And_ExpectedValue_When_Third_DropdownOption_Selected_And_DropdownValue_Updated_Then_DropdownValue_Should_Match_ExpectedValue()
        {
            // given
            var fluentSelect = Page.Locator("fluent-select#deployment-model-list");
            await fluentSelect.ClickAsync();
            var fluentOptions = fluentSelect.Locator("fluent-option");
            var expectedValue = "AZ"; // 실제 컴포넌트 3번째 옵션 값

            // when
            await fluentOptions.Nth(2).ScrollIntoViewIfNeededAsync(); // 컴포넌트 3번째 옵션 보이도록 스크롤
            await fluentOptions.Nth(2).ClickAsync(); // 옵션 클릭
            var actualValue = await Page.EvaluateAsync<string>("() => document.querySelector('fluent-select#deployment-model-list').value"); // 페이지 내 컴포넌트 값 가져오기

            // then
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [TearDown]
        public async Task CleanUp()
        {
            await Page.CloseAsync();
        }
    }
}