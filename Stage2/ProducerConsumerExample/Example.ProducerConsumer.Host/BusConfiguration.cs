using Microsoft.Extensions.DependencyInjection;
using Example.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;

namespace Example.ProducerConsumer.Host
{
    public static class BusConfiguration
    {
		public static IBusControl MessageBusSetup(IServiceProvider serviceProvider)
		{
			// here using DI container to resolve dependency.
			var brokerSettings = serviceProvider.GetService<IOptions<BrokerSettings>>().Value;
			var appSettings = serviceProvider.GetRequiredService<IOptions<ApplicationSettings>>().Value;
			
			var busControl = Bus.Factory.CreateUsingRabbitMq(cfg => {
				var host = cfg.Host(new Uri(brokerSettings.Host), h =>
				{
					h.Username(brokerSettings.UserName);
					h.Password(brokerSettings.Password);
					h.PublisherConfirmation = true;
					h.Heartbeat(30);
				});

				var currentProcess = Process.GetCurrentProcess();
				// cfg is instance of worker
				// this is used for overriding MT self generated exchanges and queues.
				cfg.OverrideDefaultBusEndpointQueueName($"bus.{appSettings.ServiceName}.{currentProcess.Id}");

				cfg.Lazy = true;
				cfg.SetQueueArgument("x-message-ttl", brokerSettings.TTL * 1000);
				cfg.PrefetchCount = 1;
				cfg.ReceiveEndpoint(
					host,
					brokerSettings.IncomingQueueName,
					ep => {
						ep.Bind(brokerSettings.IncomingQueueName,
							exchange =>
							{
								exchange.AutoDelete = false;						
								exchange.Durable = true;
							});
						ep.PrefetchCount = 1;						
						ep.Durable = true;                        
                        ep.LoadFrom(serviceProvider);						
					});
			});

			return busControl;
		}
	}
}
