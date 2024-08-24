using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using Assert = NUnit.Framework.Assert;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.UI
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ComponentIntegrationTests : PageTest
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
        // 페이지에서 컴포넌트의 헤딩이 올바르게 표시되는지 확인
        public async Task IsThereAHeading()
        {
            // Act
            await Expect(Page
                .GetByRole(AriaRole.Heading, new() { Name = "Deployment" }))
                .ToBeVisibleAsync();

            // Assert
            Assert.That(await Page.GetByRole(AriaRole.Heading, new() { Name = "Deployment" }).IsVisibleAsync(), Is.True);
        }

        [Test]
        // 페이지에서 드롭다운 컴포넌트가 올바르게 표시되는지 확인
        public async Task IsThereADropdownComponent()
        {
            // fluentSelect 컴포넌트가 페이지에 존재하는지 확인
            var fluentSelect = await Page.QuerySelectorAsync("fluent-select#deployment-model-list");
            Assert.That(fluentSelect, Is.Not.Null);
            await fluentSelect.ClickAsync();

            // fluentSelect 컴포넌트의 옵션이 페이지에 표시되는지 확인
            var fluentOptions = await fluentSelect.QuerySelectorAllAsync("fluent-option");
            Assert.That(fluentOptions.Count, Is.GreaterThan(0));
            foreach (var option in fluentOptions)
            {
                Assert.That(await option.IsVisibleAsync(), Is.True);
            }
        }

        [Test]
        // 드롭다운의 옵션 값을 선택하고, 선택된 값이 컴포넌트에 올바르게 업데이트 되는지 확인
        public async Task IsDropdownOptionSelectWorking()
        {
            // Arrange
            var fluentSelect = await Page.QuerySelectorAsync("fluent-select#deployment-model-list");
            Assert.That(fluentSelect, Is.Not.Null);
            await fluentSelect.ClickAsync();
            var fluentOptions = await fluentSelect.QuerySelectorAllAsync("fluent-option");
            Assert.That(fluentOptions.Count, Is.GreaterThan(0));

            // Act
            var userSelectedOption = fluentOptions[0]; // Select the first option
            await userSelectedOption.ClickAsync();

            await Page.EvaluateAsync(@"() => {
                window.selectValue = document.querySelector('fluent-select#deployment-model-list').value;
            }"); // Define and set 'selectValue'(Component variable) in the page context

            // Assert
            var userSelectedOptionValue = await fluentSelect.EvaluateAsync<string>("el => el.value"); // Get the selected value
            var actualSelectedValue = await Page.EvaluateAsync<string>("() => selectValue"); // Get the selected value from the page context
            Assert.That(actualSelectedValue, Is.EqualTo(userSelectedOptionValue));
        }

        [TearDown]
        public async Task Teardown()
        {
            await Page.CloseAsync();
        }
    }
}