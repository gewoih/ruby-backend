using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Enums;
using Wallet.Domain.Models.Wallet;
using Wallet.Infrastructure.Database;

namespace Wallet.Application.Services.Wallet
{
    public sealed class WalletService : IWalletService
    {
        private readonly WalletDbContext _context;

        public WalletService(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<WaxpeerPayment> CreateWaxpeerPayment(Guid userId, long steamId, List<InventoryAsset> inventoryAssets)
        {
            var waxpeerPayment = new WaxpeerPayment
            {
                SteamId = steamId,
                Amount = inventoryAssets.Sum(item => item.Price),
                UserId = userId,
                CreatedDate = DateTime.UtcNow,
                Items = inventoryAssets,
                Status = PaymentStatus.Created
            };

			await _context.WaxpeerPayments.AddAsync(waxpeerPayment);
            await _context.SaveChangesAsync();

            return waxpeerPayment;
        }

        public async Task<WaxpeerPayment?> GetActivePayment(long steamId)
        {
            //TODO: пока считаем, что у пользователя может быть только 1 незавершенный платеж
            return await _context.WaxpeerPayments.FirstOrDefaultAsync(payment => 
                payment.SteamId.Equals(steamId) &&
                payment.Status.Equals(PaymentStatus.Created));
        }

        public async Task<bool> UpdatePayment(WaxpeerPayment payment)
        {
            _context.WaxpeerPayments.Update(payment);
            var updatedRows = await _context.SaveChangesAsync();

            return updatedRows > 0;
        }
    }
}