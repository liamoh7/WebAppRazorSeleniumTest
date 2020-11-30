using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenQA.Selenium;
using RazorUITestProject.Mocks;
using RazorUITests;
using RazorUITests.Services;
using Xunit;

namespace RazorUITestProject
{
    public class HomeTest : IClassFixture<LocalFactory<Startup>>
    {
        public HomeTest(LocalFactory<Startup> appFactory)
        {
            this.CustomAppFactory = appFactory;
        }

        private LocalFactory<Startup> CustomAppFactory { get; }

        [Fact]
        public async Task Test()
        {
            var client = this.CustomAppFactory.CreateClient();

            this.CustomAppFactory.WebDriver.Navigate().GoToUrl(this.CustomAppFactory.RootUri);

            var valText = this.CustomAppFactory.WebDriver.FindElement(By.ClassName("display-4")).Text;

            Assert.Equal("Welcome: Fake Home Service: Goto Fake Home", valText);
        }
    }
}