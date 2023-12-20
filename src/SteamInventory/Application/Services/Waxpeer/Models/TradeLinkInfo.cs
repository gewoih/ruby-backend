namespace SteamInventory.Application.Services.Waxpeer.Models
{
	public sealed class TradeLinkInfo
	{
		public bool Success { get; set; }
		public string Info { get; set; }
		public string Message { get; set; }
		public string Link { get; set; }
		public string Token { get; set; }
		public string SteamId { get; set; }
	}
}
