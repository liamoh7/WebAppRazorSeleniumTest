using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RazorUITestProject
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private IWebHost _webHost;

        public string IP;
        
        public IWebDriver WebDriver { get; }

        public CustomWebApplicationFactory()
        {
            this.WebDriver = new ChromeDriver();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Find free TCP port to configure Kestrel on
            IPEndPoint endPoint;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                socket.Listen(1);
                endPoint = (IPEndPoint) socket.LocalEndPoint;
            }

            this.IP = $"{endPoint.Port}";

            // Configure testing to use Kestrel and test services
            builder
                .UseKestrel(k => k.Listen(endPoint));
        }
        
        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            // See: https://github.com/aspnet/AspNetCore/issues/4892
            this._webHost = builder.Build();

            var testServer = new TestServer(new PassthroughWebHostBuilder(this._webHost));
            var address = testServer.Host.ServerFeatures.Get<IServerAddressesFeature>();
            testServer.BaseAddress = new Uri(address.Addresses.First());

            return testServer;
        }

        private sealed class PassthroughWebHostBuilder : IWebHostBuilder
        {
            private readonly IWebHost _webHost;

            public PassthroughWebHostBuilder(IWebHost webHost)
            {
                this._webHost = webHost;
            }

            public IWebHost Build() => this._webHost;

            public IWebHostBuilder ConfigureAppConfiguration(
                Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
            {
                return this;
            }

            public IWebHostBuilder ConfigureServices(
                Action<WebHostBuilderContext, IServiceCollection> configureServices)
            {
                return this;
            }

            public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
            {
                return this;
            }

            public string GetSetting(string key) => throw new NotImplementedException();

            public IWebHostBuilder UseSetting(string key, string value)
            {
                return this;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this._webHost?.Dispose();
                this.WebDriver?.Dispose();
            }
        }
    }
}