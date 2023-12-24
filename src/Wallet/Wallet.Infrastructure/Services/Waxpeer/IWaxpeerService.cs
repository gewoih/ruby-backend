using Casino.SharedLibrary.Enums;
using Wallet.Infrastructure.Models.Waxpeer;

namespace Wallet.Infrastructure.Services.Waxpeer
{
	public interface IWaxpeerService
    {
        Task<UserInfo?> GetUserInfoAsync(long steamId);
        Task<UserInfo?> AddUserAsync(long steamId, string tradeLink);

        Task<InventoryInfo?> GetInventoryInfoAsync(long steamId);
        Task<List<WaxpeerInventoryAsset>> GetSteamAssetsAsync(long steamId, SteamGame game);

        Task<TradeLinkInfo?> GetTradeLinkInfoAsync(string tradeLink);

        Task<List<SellItemDto>> SellItemsAsync(long steamId, List<SellItemDto> items);
    }
}
