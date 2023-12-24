using Newtonsoft.Json;

namespace Wallet.Infrastructure.Models.Waxpeer
{
	public sealed class UserInfo
	{
		[JsonProperty("steam_id")]
		public string SteamId { get; set; }

		[JsonProperty("tradelink")]
		public string TradeLink { get; set; }

		[JsonProperty("can_sell")]
		public bool CanSell { get; set; }

		[JsonProperty("can_p2p")]
		public bool CanP2P { get; set; }
	}
}
