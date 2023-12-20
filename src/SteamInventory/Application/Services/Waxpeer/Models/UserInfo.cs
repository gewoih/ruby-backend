namespace SteamInventory.Application.Services.Waxpeer.Models
{
	public sealed class UserInfo
	{
		public bool IsNewUser { get; set; }
		public string SteamId { get; set; }
		public string TradeLink { get; set; }
		public bool CanSell { get; set; }
		public bool CanP2P { get; set; }
	}
}
