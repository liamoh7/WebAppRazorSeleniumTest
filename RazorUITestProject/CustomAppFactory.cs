using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RazorUITests;

namespace RazorUITestProject
{
    public class CustomAppFactory : WebApplicationFactory<Startup>
    {
        private static string _rootUrl = "http://localhost:5000";

        public IWebDriver WebDriver { get; }

        private IWebHost WebHost { get; set; }
        
        private Process Process { get; }

        public CustomAppFactory()
        {
            this.WebDriver = new ChromeDriver();

            ClientOptions.BaseAddress = new Uri(_rootUrl);

            this.Process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "razor-ui-tests",
                    Arguments = "start",
                    UseShellExecute = true,
                    CreateNoWindow = false
                }
            };

            this.Process.Start();
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            this.WebHost = builder.Build();
            this.WebHost.Start();
            
            var rootUri = this.WebHost.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault(); //Last is ssl!
            
            return new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.WebHost.Dispose();
                this.Process.CloseMainWindow();
            }
            
            this.WebDriver.Quit();
        }
    }
}