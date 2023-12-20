using SteamInventory.Application.Services.Waxpeer.Models;

namespace SteamInventory.Application.Services.Waxpeer
{
	public interface IWaxpeerService
	{
		Task<UserInfo?> GetUserInfoAsync(long steamId);
		Task<TradeLinkInfo> GetTradeLinkInfoAsync(string tradeLink);
	}
}
