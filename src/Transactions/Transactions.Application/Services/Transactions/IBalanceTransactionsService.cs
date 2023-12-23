using Casino.SharedLibrary.MessageBus.TopUp;
using MassTransit;

namespace Transactions.Application.Services.Transactions
{
	public interface IBalanceTransactionsService : IConsumer<BalanceTopUp>
	{
        Task<bool> Create(Guid userId, decimal amount);
        Task<decimal> GetUserBalance(Guid userId);
    }
}
