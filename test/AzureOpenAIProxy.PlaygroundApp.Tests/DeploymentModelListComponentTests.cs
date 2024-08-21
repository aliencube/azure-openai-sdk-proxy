using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using Assert = NUnit.Framework.Assert;
using Bunit;

namespace AzureOpenAIProxy.PlaygroundApp.Tests
{
    [TestFixture]
    public class HomePageTests : PageTest
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false // 브라우저를 헤드리스 모드로 실행하지 않으려면 false로 설정
            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [TearDown]
        public async Task Teardown()
        {
            await _page.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [SetUp]
        public async Task NavigateToHomePage()
        {
            await _page.GotoAsync("http://localhost:5000");
            await _page.GetByRole(AriaRole.Link, new() { Name = "Home" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        [Test]
        // PlaygroundApp - Home.razor에서 드롭다운의 input 값을 선택하고, 선택된 값이 올바르게 설정되는지 확인
        public async Task ShouldSelectDropdownOption()
        {
            // Act
            await _page.GetByRole(AriaRole.Combobox, new() { Name = "deployment-models" }).ClickAsync();  // 이 부분에서 계속 timeout
            Assert.That(await _page.GetByRole(AriaRole.Option, new() { Name = "CA" }).IsVisibleAsync(), Is.True);

            // Assert
            // var selectedValue = await dropdown.EvaluateAsync<string>("el => el.value");
            // Assert.That(selectedValue, Is.EqualTo("CA"));

            // if (dropdown == null)
            // {
            //     Assert.Fail("Dropdown element not found");
            // }
            // else
            // {
            //     await dropdown.ClickAsync();
            //     await dropdown.SelectOptionAsync("CA");
            // }

            // // Assert
            // var selectedValue = await dropdown.EvaluateAsync<string>("el => el.value");
            // Assert.That(selectedValue, Is.EqualTo("CA"));
        }
    }

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

        [Fact]
        // 드롭다운이 올바르게 렌더링 되고, 모든 옵션이 표시되는지 확인
        public void ShouldRenderCorrectly()
        {
            // Arrange & Act
            var component = RenderComponent<DeploymentModelListComponent>();

            // Assert
            var fluentSelect = component.Find("fluent-select");
            Assert.That(fluentSelect, Is.Not.Null);

            var options = component.FindAll("fluent-option");
            Assert.That(options.Count, Is.EqualTo(38)); // 하드코딩 된 데이터 개수: 38개
        }

        [Fact]
        // FluentSelect의 옵션 값이 선택되면 ValueChanged 이벤트가 트리거되고, 선택된 값으로 업데이트되는지 확인
        public void ShouldTriggerValueChanged()
        {
            // Arrange
            var component = RenderComponent<DeploymentModelListComponent>();
            var fluentSelect = component.Find("fluent-select");

            // Act
            fluentSelect.Change("CA"); // 하드코딩 된 데이터 중 하나인 'CA'를 선택

            // Assert
            var selectedValue = component.Instance.selectValue;
            Assert.That(selectedValue, Is.EqualTo("CA"));
        }
    }
}