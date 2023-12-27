using Casino.SharedLibrary.MessageBus.Transactions;

namespace Transactions.Domain.Models
{
	public sealed class BalanceTransaction
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public Guid TriggerId { get; set; }
		public TransactionTriggerType TriggerType { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public decimal Amount { get; set; }
	}
}
