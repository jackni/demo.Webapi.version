using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Example.ProducerConsumer.Host
{
    static class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = configBuilder.Build();
            var serviceCollection = new ServiceCollection();

            Bootstrap.ConfigureServices(serviceCollection, config);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<App>().Run();
        }
    }
}
