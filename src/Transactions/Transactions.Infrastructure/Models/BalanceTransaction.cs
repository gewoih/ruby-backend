﻿namespace Transactions.Infrastructure.Models
{
	public sealed class BalanceTransaction
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public DateTime CreatedDate { get; set; }
		public decimal AdjustmentAmount { get; set; }
	}
}