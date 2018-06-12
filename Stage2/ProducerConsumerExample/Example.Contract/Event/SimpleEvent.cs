using System;

namespace Example.Contract.Event
{
    public class SimpleEvent : ISimpleEvent
    {
        public Guid ConversationId { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
