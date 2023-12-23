using Newtonsoft.Json;

namespace SteamInventory.Application.Models.Waxpeer
{
	public sealed class SoldItemDto
	{
		[JsonProperty("status")]
		public int Status { get; set; }

		[JsonProperty("item_id")]
		public long ItemId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("currency_value")]
		public decimal Price { get; set; }

		[JsonProperty("img")]
		public string ImageUrl { get; set; }
	}
}
