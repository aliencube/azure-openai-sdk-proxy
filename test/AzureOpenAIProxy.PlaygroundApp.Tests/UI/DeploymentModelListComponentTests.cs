using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using Assert = NUnit.Framework.Assert;
using Bunit;
using AzureOpenAIProxy.PlaygroundApp.Components.UI;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.UI
{
    // 1. Playwright 테스트
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ComponentIntegrationTests : PageTest
    {
        public override BrowserNewContextOptions ContextOptions() => new()
        {
            IgnoreHTTPSErrors = true,
        };

        [SetUp]
        public async Task NavigateToTargetPage()
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

    // 2. Bunit 테스트
    [TestFixture]
    public class DeploymentModelListComponentTests : Bunit.TestContext
    {
        public DeploymentModelListComponentTests()
        {
            // FluentUI 구성 요소를 위한 LibraryConfiguration 인스턴스 생성 및 등록
            var libraryConfiguration = new LibraryConfiguration();
            Services.AddSingleton(libraryConfiguration);

            // FluentUI 구성 요소 서비스 추가
            Services.AddFluentUIComponents();

            // JSInterop 설정
            JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [Test]
        // 드롭다운이 올바르게 렌더링 되고, 옵션이 표시되는지 확인
        public void ShouldRenderCorrectly()
        {
            // Arrange & Act
            var cut = RenderComponent<DeploymentModelListComponent>();
            var fluentSelect = cut.Find("fluent-select#deployment-model-list");

            // Assert
            Assert.That(fluentSelect, Is.Not.Null);

            var fluentOptions = cut.FindAll("fluent-option");
            Assert.That(fluentOptions.Count, Is.GreaterThan(0));
        }

        [Test]
        // FluentSelect의 옵션이 선택되면 ValueChanged 이벤트가 트리거되고, 선택된 옵션 값으로 업데이트되는지 확인
        public void ShouldTriggerValueChanged()
        {
            // Arrange
            var cut = RenderComponent<DeploymentModelListComponent>();
            var fluentSelect = cut.Find("fluent-select#deployment-model-list");

            // Act
            var fluentOptions = fluentSelect.QuerySelectorAll("fluent-option");
            var actualSelectedValue = fluentOptions[0].GetAttribute("value");

            // Define and set selectValue in the page context
            fluentOptions[0].Click(); // 첫 번째 옵션을 선택 - 여기서 value가 update 되어야 함
            var updatedSelectedValue = cut.Instance.selectValue;

            // Assert
            Assert.That(actualSelectedValue, Is.EqualTo(updatedSelectedValue));
        }
    }
}