using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RazorUITestProject
{
    public class LocalFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private const string LocalhostBaseAddress = "https://localhost";
        
        public IWebDriver WebDriver { get; }

        public string RootUri { get; private set; }

        private IWebHost _host;

        public LocalFactory()
        {
            this.WebDriver = new ChromeDriver();
            
            ClientOptions.BaseAddress = new Uri(LocalhostBaseAddress);

            CreateServer(CreateWebHostBuilder());
        }
        
        protected sealed override TestServer CreateServer(IWebHostBuilder builder)
        {
            _host = builder.Build();
            _host.Start();
            
            RootUri = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();
            
            // not used but needed in the CreateServer method logic
            return new TestServer(new WebHostBuilder().UseStartup<TStartup>());
        }
        protected sealed override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = WebHost.CreateDefaultBuilder(Array.Empty<string>());
            builder.UseStartup<TStartup>();
            return builder;
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            if (!disposing) return;
            
            this.WebDriver?.Dispose();
            _host?.Dispose();
        }
    }
}