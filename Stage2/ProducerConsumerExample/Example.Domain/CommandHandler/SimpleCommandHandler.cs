using Example.Contract.Command;
using Microsoft.Extensions.Logging;

namespace Example.Domain.CommandHandler
{
    public class SimpleCommandHandler : ISimpleCommandHandler
    {
        private readonly ILogger<SimpleCommandHandler> _logger;

        public SimpleCommandHandler(ILogger<SimpleCommandHandler> logger)
        {
            _logger = logger;
        }

        public void ProcessCommand(ISimpleCommand command)
        {
            //all I need to deal with is the payload, composing over inheritance benifit is here
            var payload = command as IPayload;
            //out put to console log
            _logger.LogInformation($"procss command payload: {payload.SimpleMessage}");
        }
    }
}
