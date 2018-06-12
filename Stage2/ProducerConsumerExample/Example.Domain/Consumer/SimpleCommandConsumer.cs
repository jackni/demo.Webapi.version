using System.Threading.Tasks;
using Example.Contract.Command;
using Example.Contract.Event;
using Example.Domain.CommandHandler;
using MassTransit;
using Example.Common.CommandValidation;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Example.Settings;
using System;
using FluentValidation;
using Newtonsoft.Json;

namespace Example.Domain.Consumer
{
	public class SimpleCommandConsumer : IConsumer<ISimpleCommand>
	{
        private readonly ISimpleCommandHandler _commandHandler;
        private readonly IEventProcessor _eventProcessor;
		private readonly IValidator<IPayload> _payloadValidator;
		private readonly ILogger<SimpleCommandConsumer> _logger;
		private readonly BrokerSettings _brokerSettings;


		public SimpleCommandConsumer(
            ISimpleCommandHandler commandHandler,
            IEventProcessor eventProcessor,
			IValidator<IPayload> payloadValidator,
			ILogger<SimpleCommandConsumer> logger,
			IOptions<BrokerSettings> brokerSettings)
        {
            _commandHandler = commandHandler;
            _eventProcessor = eventProcessor;
			_payloadValidator = payloadValidator;
			_logger = logger;
			_brokerSettings = brokerSettings.Value;
        }

        public async Task Consume(ConsumeContext<ISimpleCommand> context)
		{
			// contract validation
			var msgConversationId = context.Message.ConversationId;
			var msgCorrelationId = context.Message.CorrelationId;
			var validationResult = CommandValidator.GetValidationResult<SimpleCommand>(context.Message);
			if (!validationResult.IsValid)
			{
				string warningMsg = $"Process Email Command Type mismatched, ConversationId: {msgConversationId} CorrelationId: {msgCorrelationId} "
					+ $"reasons are: {validationResult.ToString()}";
				_logger.LogWarning(warningMsg);
				Thread.Sleep(500);
				// here is depends on what we want, we can do something we like. 
				// customized life cycle count can also be done here
				await context.Forward(new Uri(_brokerSettings.DeadLetterEndpoint), context.Message);
				return;
			}

			// domain validation
			var payload = context.Message as IPayload;
			ValidatePayload(payload);

			await Task.Run(()=> _commandHandler.ProcessCommand(context.Message));
            await _eventProcessor.PublishSimpleEvent<ISimpleCommand>(
                context,
                new SimpleEvent() {
                    ConversationId = context.Message.ConversationId,
                    CorrelationId = context.Message.CorrelationId
                });
        }
		
		private void ValidatePayload(IPayload payload)
		{
			var domainValidationResult = _payloadValidator.Validate(payload);
			if (!domainValidationResult.IsValid)
			{
				string errorJson = JsonConvert.SerializeObject(domainValidationResult.Errors, Formatting.Indented);
				throw new ArgumentException($"invalid email payload: {errorJson}");
			}
		}
	}
}
