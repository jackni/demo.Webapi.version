using System;
using System.Threading.Tasks;
using Example.Contract.Command;
using Example.Domain.CommandHandler;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example.ProducerConsumer.WebApi.Controllers
{
	[Produces("application/json")]
	[Route("api/{version:apiVersion}/SimpleCommand")]
	public class SimpleCommandController : Controller
    {
		private readonly ISimpleCommandHandler _simpleCommandHandler;
		private readonly IValidator<IPayload> _domainValidator;
		private readonly ILogger<SimpleCommandController> _logger;

		public SimpleCommandController(
			ISimpleCommandHandler simpleCommandHandler, 
			IValidator<IPayload> domainValidator,
			ILogger<SimpleCommandController> logger)
		{
			_simpleCommandHandler = simpleCommandHandler;
			_domainValidator = domainValidator;
			_logger = logger;
		}

		[HttpPost, MapToApiVersion("1.0")]
		[Route("SendCommandAsync")]
		[ApiExplorerSettings(GroupName = "SimpleCommand")]
		public async Task<IActionResult> SendCommandAsync([FromBody]SimpleCommand command)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			//domain validation
			var payload = command as IPayload;
			var domainResult = _domainValidator.Validate(payload);
			if (!domainResult.IsValid)
			{
				string errMsg = $"Payload Error, ConversationId: {command.ConversationId} CorrelationId: {command.CorrelationId} "
					+ $"reasons are: {domainResult.Errors.ToString()}";
				_logger.LogError(errMsg);
				// currently I will go to the fault queue, we can deal with this in different way later
				throw new InvalidOperationException(errMsg);
			}
			command.Source += ".webapi";
			_simpleCommandHandler.ProcessCommand(command);
			return Ok();
		}
	}
}