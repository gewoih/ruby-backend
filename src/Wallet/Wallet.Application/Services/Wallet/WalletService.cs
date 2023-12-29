using Casino.SharedLibrary.MessageBus.Transactions;
using Casino.SharedLibrary.Services.MessageBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Enums;
using Wallet.Domain.Models.Wallet;
using Wallet.Infrastructure.Database;
using Wallet.Infrastructure.Models.Waxpeer;

namespace Wallet.Application.Services.Wallet
{
	public sealed class WalletService : IWalletService
    {
        private readonly WalletDbContext _context;
        private readonly IMessageBusService _messageBusService;

        public WalletService(WalletDbContext context, IMessageBusService messageBusService)
        {
            _context = context;
            _messageBusService = messageBusService;
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

        public async Task<bool> CompletePayment(WaxpeerPayment payment, SoldItemsWebhookDto soldItemsDto)
        {
            payment.Status = PaymentStatus.Completed;
            _context.WaxpeerPayments.Update(payment);
            var updatedRows = await _context.SaveChangesAsync();
            
            await CreateAndPublishDepositMessage(payment, soldItemsDto);
            
            return updatedRows > 0;
        }

        private async Task CreateAndPublishDepositMessage(WaxpeerPayment payment, SoldItemsWebhookDto soldItemsDto)
        {
            var paymentMessage = new TransactionTrigger
            {
                Amount = soldItemsDto.Items
                    .Where(item => item.Status.Equals(5))
                    .Sum(item => item.Price),

                Type = TransactionTriggerType.Payment,
                UserId = payment.UserId,
                TriggerId = payment.Id
            };
            
            await _messageBusService.Publish(paymentMessage);
        }
    }
}