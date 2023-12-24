using Microsoft.EntityFrameworkCore;
using Transactions.Domain.Models;

namespace Transactions.Infrastructure.Database
{
	public sealed class TransactionsDbContext : DbContext
	{
		public DbSet<BalanceTransaction> BalanceTransactions { get; set; }

        public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : base(options)
        {
        }
    }
}
