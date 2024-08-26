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
            await Expect(componentLabel).ToBeVisibleAsync();
            
            // when
            var expected = await Page.GetByText("Deployment").IsVisibleAsync();

            // then
            Assert.That(expected, Is.True);
        }

        [Test]
        // 페이지에서 드롭다운 컴포넌트가 올바르게 표시되는지 확인
        public async Task Given_Dropdown_When_Waiting_Then_ShouldBeVisible()
        {
            // given
            // var fluentSelect = Page.GetByRole(AriaRole.Combobox, new() { Name = "Deployment" });
            var fluentSelect = Page.Locator("fluent-select#deployment-model-list");
            await Expect(fluentSelect).ToBeVisibleAsync();

            // when
            var expected = await fluentSelect.IsVisibleAsync();
            
            // then
            Assert.That(expected, Is.True);
        }

        [Test]
        // 드롭다운의 옵션 값이 존재하는지 확인
        public async Task Given_Dropdown_When_CountingOption_Then_ShouldOptionExist()
        {
            // given
            var fluentSelect = Page.Locator("fluent-select#deployment-model-list");
            await Expect(fluentSelect).ToBeVisibleAsync();
            await fluentSelect.ClickAsync();

            // when
            int count = await fluentSelect.Locator("fluent-option").CountAsync();
            //int count = await Page.QuerySelectorAllAsync("fluent-option").Count();

            // then
            Assert.That(count, Is.GreaterThan(0));
        }

        [Test]
        // 드롭다운의 옵션 값을 선택하면 부모 컴포넌트(페이지 컴포넌트)에 올바르게 업데이트 되는지 확인
        public async Task Given_Dropdown_When_OptionSelected_Then_ShouldUpdateComponentValue()
        {
            // given
            var fluentSelect = Page.Locator("fluent-select#deployment-model-list");
            await Expect(fluentSelect).ToBeVisibleAsync();
            var expectedValue = "AZ"; // 컴포넌트 3번째 옵션 값
            //var fluentOptions = fluentSelect.Locator("fluent-option");
            await fluentSelect.ClickAsync(); // 드롭다운 클릭
            await fluentSelect.Locator("fluent-option").Nth(2).ScrollIntoViewIfNeededAsync(); // 3번째 옵션 보이도록 스크롤
            await fluentSelect.Locator("fluent-option").Nth(2).ClickAsync(); // 옵션 클릭

            // when
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