using Wallet.Domain.Models.Wallet;
using Wallet.Infrastructure.Models.Waxpeer;

namespace Wallet.Application.Services.Wallet
{
    public interface IWalletService
    {
        Task<WaxpeerPayment> CreateWaxpeerPayment(Guid userId, long steamId, List<InventoryAsset> inventoryAssets);
        Task<WaxpeerPayment?> GetActivePayment(long steamId);
        Task<bool> CompletePayment(WaxpeerPayment payment, SoldItemsWebhookDto soldItemsDto);
    }
}