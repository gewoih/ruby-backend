using Casino.SharedLibrary.Enums;
using Wallet.Domain.Models.Payments.Waxpeer;
using Wallet.Infrastructure.Models.Waxpeer;

namespace Wallet.Application.Utils
{
    public static class InventoryAssetsUtils
	{
		public static List<InventoryAsset> ToInventoryAssets(this List<SellItemDto> listedItems, Guid userId)
		{
			return listedItems.Select(item => new InventoryAsset
			{
				UserId = userId,
				SteamGame = SteamGame.CounterStrike,
				Price = item.Price,
				Name = item.Name,
				AssetId = item.ItemId
			}).ToList();
		}
	}
}
