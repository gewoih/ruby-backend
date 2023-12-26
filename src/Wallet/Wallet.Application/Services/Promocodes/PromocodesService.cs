using MassTransit;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Enums;
using Wallet.Domain.Models;
using Wallet.Infrastructure.Database;

namespace Wallet.Application.Services.Promocodes
{
	public sealed class PromocodesService : IPromocodesService
    {
        private readonly WalletDbContext _context;
        private readonly IBus _messagesBus;

        public PromocodesService(WalletDbContext context, IBus messagesBus)
        {
            _context = context;
            _messagesBus = messagesBus;
        }

        public async Task<bool> ActivateAsync(Guid userId, string code)
        {
            var promocode = await _context.Promocodes.FirstOrDefaultAsync(promocode => 
                promocode.Code.Equals(code) && promocode.UserId.Equals(userId));

            if (!Validate(userId, promocode))
                return false;

            promocode.IsActivated = true;
            promocode.ActivatedDateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            await _messagesBus.Publish(promocode);

            return true;
        }

        public async Task<Promocode> CreateAsync(Guid userId, PromocodeType type, TimeSpan lifeSpan, int amount, bool isRandomCode)
        {
            var code = GenerateCode(type, amount, isRandomCode);
            var promocode = new Promocode
            {
                UserId = userId,
                Type = type,
                Amount = amount,
                Code = code,
                IsActivated = false,
                ValidTillDateTime = DateTime.UtcNow.Add(lifeSpan),
                CreatedDateTime = DateTime.UtcNow
            };

            await _context.Promocodes.AddAsync(promocode);
            await _context.SaveChangesAsync();

            return promocode;
        }

        private static string GenerateCode(PromocodeType type, int amount, bool isRandomCode)
        {
            if (isRandomCode)
            {
                var random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }

            if (type == PromocodeType.Instant)
                return $"INST{amount}";
            
            return $"{type}{amount}";
        }

        private static bool Validate(Guid userId, Promocode? promocode)
        {
            return promocode is not null && 
                   !promocode.IsActivated && 
                   promocode.ValidTillDateTime >= DateTime.UtcNow && 
                   userId.Equals(promocode.UserId);
        }
    }
}
