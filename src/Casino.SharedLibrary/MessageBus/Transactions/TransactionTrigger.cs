namespace Casino.SharedLibrary.MessageBus.Transactions
{
	public sealed class TransactionTrigger
	{
        public Guid UserId { get; set; }
		public TransactionTriggerType Type { get; set; }
        public Guid TriggerId { get; set; }
        public decimal Amount { get; set; }
	}
}
