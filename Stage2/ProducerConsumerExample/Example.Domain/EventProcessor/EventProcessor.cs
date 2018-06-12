using Example.Contract.Event;
using MassTransit;
using System.Threading.Tasks;

namespace Example.Domain
{
    public class EventProcessor : IEventProcessor
    {
        public async Task PublishSimpleEvent<T>(
            ConsumeContext<T> context,
            SimpleEvent simpleEvent) where T : class
        {
            await context.Publish<ISimpleEvent>(simpleEvent);
        }

        public async Task PublishSimpleErrorEvent<T>(
            ConsumeContext<Fault<T>> context,
            SimpleErrorEvent simpleErrorEvent) where T : class
        {
            await context.Publish<ISimpleErrorEvent>(simpleErrorEvent);
        }
    }
}
