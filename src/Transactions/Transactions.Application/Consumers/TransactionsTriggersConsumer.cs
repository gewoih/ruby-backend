using Casino.SharedLibrary.MessageBus.Transactions;
using MassTransit;
using Transactions.Domain.Models;
using Transactions.Infrastructure.Database;

namespace Transactions.Application.Consumers
{
	public sealed class TransactionsTriggersConsumer : IConsumer<TransactionTrigger>
	{
        private readonly TransactionsDbContext _context;

        public TransactionsTriggersConsumer(TransactionsDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<TransactionTrigger> context)
        {
            var balanceTransaction = new BalanceTransaction
            {
                UserId = context.Message.UserId,
                TriggerId = context.Message.TriggerId,
                TriggerType = context.Message.Type,
                Amount = context.Message.Amount,
                CreatedDateTime = DateTime.UtcNow
            };

            await _context.BalanceTransactions.AddAsync(balanceTransaction);
            await _context.SaveChangesAsync();
        }
	}
}
