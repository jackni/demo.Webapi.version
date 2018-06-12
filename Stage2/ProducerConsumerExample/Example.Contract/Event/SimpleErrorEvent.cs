using System;

namespace Example.Contract.Event
{
    public class SimpleErrorEvent : ISimpleErrorEvent
    {
        public Guid ConversationId { get; set; }

        public Guid CorrelationId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
