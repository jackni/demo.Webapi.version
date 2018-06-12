using Example.Contract.Command;

namespace Example.Domain.CommandHandler
{
    public interface ISimpleCommandHandler
    {
        void ProcessCommand(ISimpleCommand command);
    }
}