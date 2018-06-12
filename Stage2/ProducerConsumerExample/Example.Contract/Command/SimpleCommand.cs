using System;
using System.ComponentModel.DataAnnotations;

namespace Example.Contract.Command
{
    public class SimpleCommand : ISimpleCommand
    {
        public Guid ConversationId { get; set; }

        public Guid CorrelationId { get; set; }

        public string Source { get; set; }

		[Required]
		[MaxLength(20,ErrorMessage = "simple message is less than 20 chars")]
        public string SimpleMessage { get; set; }
		
		[EmailAddress]
		public string EmailAddress { get; set; }

		//this can be done at contract validation, but we use it for domain validation
		public int NoneNegtiveValue { get; set; }
	}
}
