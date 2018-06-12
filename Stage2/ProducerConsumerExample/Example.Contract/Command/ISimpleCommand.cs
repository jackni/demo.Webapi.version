namespace Example.Contract.Command
{
	// http://masstransit-project.com/MassTransit/usage/message-contracts.html
	// here I construct command in favor of composing over inheritance.
	// reason: https://en.wikipedia.org/wiki/Composition_over_inheritance
	// Masstransit using immutable contract concept.
	public interface ISimpleCommand: IBusCommand, IPayload
    {
    }
}
