using Wallet.Application.Models;
using Wallet.Application.Models.Inventory;
using Wallet.Application.Models.Waxpeer;

namespace Wallet.Application.Services.Waxpeer
{
	public interface IWaxpeerService
	{
		Task<UserInfo?> GetUserInfoAsync(long steamId);
		Task<UserInfo?> AddUserAsync(long steamId, string tradeLink);

		Task<InventoryInfo?> GetInventoryInfoAsync(long steamId);
		Task<List<InventoryAsset>> GetSteamAssetsAsync(long steamId, SteamGame game);

		Task<TradeLinkInfo?> GetTradeLinkInfoAsync(string tradeLink);

		Task<List<WaxpeerItem>> SellItemsAsync(string steamId, List<WaxpeerItem> items);
	}
}
