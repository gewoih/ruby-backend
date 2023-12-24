namespace Transactions.Domain.Models
{
	public sealed class BalanceTransaction
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public Guid PaymentId { get; set; }
		public DateTime CreatedDate { get; set; }
		public decimal AdjustmentAmount { get; set; }
	}
}
