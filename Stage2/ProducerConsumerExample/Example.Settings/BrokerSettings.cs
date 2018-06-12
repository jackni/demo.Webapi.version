namespace Example.Settings
{
    public class BrokerSettings
    {
		public const string Incoming = "Incoming";
		public const string Outgoing = "Outgoing";
		public string Host { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ExchangeName { get; set; }
		public int TTL { get; set; }
		public string DeadLetterEndpoint { get; set; }
		public string IncomingQueueName => $"{ExchangeName}.{Incoming}";
		public string OutgoingQueueName => $"{ExchangeName}.{Outgoing}";
	}
}
