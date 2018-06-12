using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Contract.Command
{
    public interface IBusCommand
    {
		Guid ConversationId { get; }
		Guid CorrelationId { get; }
		string Source { get; }
	}
}
