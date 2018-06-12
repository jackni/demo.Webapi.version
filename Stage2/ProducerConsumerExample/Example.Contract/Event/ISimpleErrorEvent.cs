using System;

namespace Example.Contract.Event
{
    public interface ISimpleErrorEvent
    {
        Guid ConversationId { get; }
        Guid CorrelationId { get; }

        string ErrorMessage { get; }
    }
}
