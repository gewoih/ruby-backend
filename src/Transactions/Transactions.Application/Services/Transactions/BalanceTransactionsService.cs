using Microsoft.EntityFrameworkCore;
using Transactions.Domain.Models;
using Transactions.Infrastructure.Database;

namespace Transactions.Application.Services.Transactions
{
	public sealed class BalanceTransactionsService : IBalanceTransactionsService
	{
        private readonly TransactionsDbContext _context;

        public BalanceTransactionsService(TransactionsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Guid userId, decimal amount)
        {
            var balanceTransaction = new BalanceTransaction
            {
                CreatedDateTime = DateTime.UtcNow,
                UserId = userId,
                Amount = amount
            };

            await _context.BalanceTransactions.AddAsync(balanceTransaction);
            var changedRows = await _context.SaveChangesAsync();
            return changedRows > 0;
        }

        public async Task<decimal> GetUserBalance(Guid userId)
        {
            var balance = await _context.BalanceTransactions
                .AsNoTracking()
                .Where(transaction => transaction.UserId.Equals(userId))
                .SumAsync(transaction => transaction.Amount);

            return balance;
        }
    }
}
