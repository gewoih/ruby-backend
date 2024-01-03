using Wallet.Domain.Models.Payments.Waxpeer;
using Wallet.Infrastructure.Models.Waxpeer;

namespace Wallet.Application.Services.Payments.Waxpeer
{
    public interface IWaxpeerPaymentService
    {
        Task<WaxpeerPayment> CreateWaxpeerPayment(Guid userId, long steamId, List<InventoryAsset> inventoryAssets);
        Task<WaxpeerPayment?> GetActivePayment(long steamId);
        Task<bool> CompletePayment(WaxpeerPayment payment, SoldItemsWebhookDto soldItemsDto);
    }
}