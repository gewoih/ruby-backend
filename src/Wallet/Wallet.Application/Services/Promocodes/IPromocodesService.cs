using Wallet.Domain.Enums;
using Wallet.Domain.Models;

namespace Wallet.Application.Services.Promocodes
{
	public interface IPromocodesService
    {
        Task<bool> ActivateAsync(Guid userId, string code);
        Task<Promocode> CreateAsync(Guid userId, PromocodeType type, TimeSpan lifeSpan, int amount, bool isRandomCode);
    }
}
