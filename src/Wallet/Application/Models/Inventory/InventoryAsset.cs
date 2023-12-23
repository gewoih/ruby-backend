using Wallet.Application.Models;

namespace Wallet.Application.Models.Inventory
{
	public sealed class InventoryAsset
	{
		public long SteamUserId { get; set; }
		public SteamGame SteamGame { get; set; }
		public long AssetId { get; set; }
		public string MarketName { get; set; }
		public bool Marketable { get; set; }
		public string ImageUrl { get; set; }
		public double MarketPrice { get; set; }
	}
}
