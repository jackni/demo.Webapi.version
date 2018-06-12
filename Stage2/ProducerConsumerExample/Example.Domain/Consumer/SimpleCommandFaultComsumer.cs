using Example.Contract.Command;
using Example.Contract.Event;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Example.Domain.Consumer
{
    public class SimpleCommandFaultComsumer : IConsumer<Fault<ISimpleCommand>>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<SimpleCommandFaultComsumer> _logger;

        public SimpleCommandFaultComsumer(
            IEventProcessor eventProcessor,
            ILogger<SimpleCommandFaultComsumer> logger
            )
        {
            _eventProcessor = eventProcessor;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Fault<ISimpleCommand>> context)
        {
            var consumerEx = JsonConvert.SerializeObject(context.Message.Exceptions);
            _logger.LogCritical(consumerEx);
            await _eventProcessor.PublishSimpleErrorEvent<ISimpleCommand>(
                context,
                new SimpleErrorEvent()
                {
                    ConversationId = context.Message.Message.ConversationId,
                    CorrelationId = context.Message.Message.CorrelationId,
                    ErrorMessage = consumerEx
                });
        }
    }
}
