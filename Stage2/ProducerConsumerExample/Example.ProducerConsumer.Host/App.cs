
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;
using MassTransit.Util;

namespace Example.ProducerConsumer.Host
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IBusControl _bus;

        public App(ILogger<App> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _bus = serviceProvider.GetService<IBusControl>();
        }

        public void Run()
        {
            _logger.LogInformation("your first app running on message bus starting");
            TaskUtil.Await(() => _bus.StartAsync());
            _logger.LogInformation("your first app running on message bus started");
        }
    }
}
