using Transactions.Application.Enums;

namespace Transactions.Infrastructure.Models
{
	public sealed class BalanceTransaction
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public DateTime CreatedDate { get; set; }
		public TransactionType Type { get; set; }
		public decimal AdjustmentAmount { get; set; }
	}
}
