namespace Transactions.Application.Services.Transactions
{
	public interface IBalanceTransactionsService
	{
        Task<bool> Create(Guid userId, decimal amount);
        Task<decimal> GetUserBalance(Guid userId);
    }
}
