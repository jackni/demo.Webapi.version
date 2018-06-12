using System.Threading.Tasks;
using Example.Contract.Event;
using MassTransit;

namespace Example.Domain
{
    public interface IEventProcessor
    {
        Task PublishSimpleErrorEvent<T>(ConsumeContext<Fault<T>> context, SimpleErrorEvent simpleErrorEvent) where T : class;
        Task PublishSimpleEvent<T>(ConsumeContext<T> context, SimpleEvent simpleEvent) where T : class;
    }
}