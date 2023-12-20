using SteamInventory.Application.Models;
using SteamInventory.Application.Models.Inventory;
using SteamInventory.Application.Services.Waxpeer.Models;

namespace SteamInventory.Application.Services.Waxpeer
{
	public interface IWaxpeerService
	{
		Task<UserInfo?> GetUserInfoAsync(long steamId);
		Task<UserInfo?> AddUserAsync(long steamId, string tradeLink);

		Task<InventoryInfo?> GetInventoryInfoAsync(long steamId);
		Task<List<WaxpeerAsset>> GetSteamAssetsAsync(long steamId, SteamGame game);

		Task<TradeLinkInfo?> GetTradeLinkInfoAsync(string tradeLink);
	}
}
