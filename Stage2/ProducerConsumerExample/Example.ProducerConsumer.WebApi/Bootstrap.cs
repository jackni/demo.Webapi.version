using Example.Contract.Command;
using Example.Domain;
using Example.Domain.CommandHandler;
using Example.Domain.Consumer;
using Example.Domain.Validation;
using Example.Settings;
using FluentValidation;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Example.ProducerConsumer.WebApi
{
    //DI registration here
    public static class Bootstrap
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, IConfiguration config)
        {
            // add logging
            serviceCollection.AddSingleton(new LoggerFactory().AddConsole());
            serviceCollection.AddLogging();

            // configuration inject
            serviceCollection.AddOptions()
                .Configure<ApplicationSettings>(config.GetSection("settings"));
            serviceCollection.AddOptions()
                .Configure<BrokerSettings>(config.GetSection("brokerSettings"));

            // configuration message bus
            serviceCollection.AddSingleton(BusConfiguration.MessageBusSetup);
			serviceCollection.AddTransient<IValidator<IPayload>, PayloadValidator>();
			serviceCollection.AddTransient<IEventProcessor, EventProcessor>();
            serviceCollection.AddTransient<ISimpleCommandHandler, SimpleCommandHandler>();
            // this may not need in MT 5
            serviceCollection.AddScoped<SimpleCommandConsumer>();
            serviceCollection.AddScoped<SimpleCommandFaultComsumer>();

            serviceCollection.AddMassTransit(mt =>
            {
                mt.AddConsumer<SimpleCommandConsumer>();
                mt.AddConsumer<SimpleCommandFaultComsumer>();
            });
        }
    }
}
