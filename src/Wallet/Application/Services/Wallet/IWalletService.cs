using Wallet.Domain.Models;

namespace Wallet.Application.Services.Wallet
{
    public interface IWalletService
    {
        Task AddWaxpeerPayment(WaxpeerPayment payment);
        Task<WaxpeerPayment?> GetActivePayment(long steamId);
    }
}