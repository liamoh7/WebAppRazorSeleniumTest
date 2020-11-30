using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using RazorUITestProject.Mocks;
using RazorUITests;
using RazorUITests.Services;

namespace RazorUITestProject.Factories
{
    public class HomeWebApplicationFactory : LocalFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder()
                .ConfigureTestServices(ConfigureMockServices());
        }

        private static Action<IServiceCollection> ConfigureMockServices()
        {
            return serviceCollection =>
            {
                var descriptor2 = new ServiceDescriptor(typeof(IHomeService), typeof(MockHomeService),
                    ServiceLifetime.Transient);

                serviceCollection.AddTransient<IHomeService, MockHomeService>();
            };
        }

        private ServiceDescriptor CreateMock(Type interfaceType, Type implementationType, ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(interfaceType, implementationType, lifetime);
        }
    }
}