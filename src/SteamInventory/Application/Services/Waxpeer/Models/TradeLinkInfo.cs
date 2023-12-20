namespace SteamInventory.Application.Services.Waxpeer.Models
{
	public sealed class TradeLinkInfo
	{
		public bool Success { get; set; }
		public string Info { get; set; }
		public string Msg { get; set; }
		public string Link { get; set; }
		public string Steamid64 { get; set; }
	}
}
