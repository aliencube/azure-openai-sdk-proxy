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
        public async Task Given_ComponentLabel_When_Waiting_Then_ShouldBeVisible()
        {
            // given
            var componentLabel = Page.GetByText("Deployment");
            
            // when & then
            await Expect(componentLabel).ToBeVisibleAsync();
        }

        [Test]
        // 페이지에서 드롭다운 컴포넌트가 올바르게 표시되는지 확인
        public async Task Given_Dropdown_When_Waiting_Then_ShouldBeVisible()
        {
            // given
            var fluentSelect = Page.Locator("fluent-select#deployment-model-list");

            // when & then
            await Expect(fluentSelect).ToBeVisibleAsync();
        }

        [Test]
        // 드롭다운의 옵션 값이 존재하는지 확인
        public async Task Given_Dropdown_When_ClickToShowOption_Then_ShouldOptionExist()
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
        public async Task Given_DropdownOption_When_OptionSelected_Then_ShouldUpdateComponentValue()
        {
            // given
            var fluentSelect = Page.Locator("fluent-select#deployment-model-list");
            var expectedValue = "AZ"; // 컴포넌트 3번째 옵션 값
            await fluentSelect.ClickAsync();
            var fluentOptions = fluentSelect.Locator("fluent-option");

            // when
            await fluentOptions.Nth(2).ScrollIntoViewIfNeededAsync(); // 3번째 옵션 보이도록 스크롤
            await fluentOptions.Nth(2).ClickAsync(); // 옵션 클릭
            var actualValue = await Page.EvaluateAsync<string>("() => document.querySelector('fluent-select#deployment-model-list').value"); // 페이지 내 컴포넌트 값 가져오기

            // then
            Assert.That(expectedValue, Is.EqualTo(actualValue));
        }

        [TearDown]
        public async Task Teardown()
        {
            await Page.CloseAsync();
        }
    }
}