using Wallet.Domain.Models;

namespace Wallet.Application.Services.Wallet
{
    public interface IWalletService
    {
        Task<WaxpeerPayment> CreateWaxpeerPayment(Guid userId, long steamId, List<InventoryAsset> inventoryAssets);
        Task<WaxpeerPayment?> GetActivePayment(long steamId);
        Task<bool> UpdatePayment(WaxpeerPayment payment);
    }
}