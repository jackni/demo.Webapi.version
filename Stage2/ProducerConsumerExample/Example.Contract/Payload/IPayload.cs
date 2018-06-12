namespace Example.Contract.Command
{
    public interface IPayload
    {
		string SimpleMessage { get; }

		string EmailAddress { get; }

		int NoneNegtiveValue { get; }
    }
}
