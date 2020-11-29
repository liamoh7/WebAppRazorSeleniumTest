using System.Threading.Tasks;
using OpenQA.Selenium;
using RazorUITests;
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

            Assert.True(false);
            // var result = await client.GetAsync("/");

            // Assert.True(result.IsSuccessStatusCode);
        }
        
    }
}