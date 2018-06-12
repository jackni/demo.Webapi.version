using System;

namespace Example.Contract.Event
{
    public interface ISimpleEvent
    {
        Guid ConversationId { get; }
        Guid CorrelationId { get; }
        
    }
}
