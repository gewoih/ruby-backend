using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Enums;
using Wallet.Domain.Models;
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

        public async Task AddWaxpeerPayment(WaxpeerPayment payment)
        {
            await _context.WaxpeerPayments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<WaxpeerPayment?> GetActivePayment(long steamId)
        {
            //TODO: пока считаем, что у пользователя может быть только 1 незавершенный платеж
            return await _context.WaxpeerPayments.FirstOrDefaultAsync(payment => 
                payment.SteamId.Equals(steamId) &&
                payment.Status.Equals(PaymentStatus.Created));
        }
    }
}