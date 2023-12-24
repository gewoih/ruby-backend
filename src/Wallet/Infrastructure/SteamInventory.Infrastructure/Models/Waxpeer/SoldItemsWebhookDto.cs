using Newtonsoft.Json;

namespace Wallet.Infrastructure.Models.Waxpeer
{
	public sealed class SoldItemsWebhookDto
	{
		[JsonProperty("costum_id")]
		public Guid CostumId { get; set; }

		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("sign")]
		public string Sign { get; set; }

		[JsonProperty("steamid")]
		public long SteamId { get; set; }

		[JsonProperty("items")]
		public List<SoldItemDto> Items { get; set; }
	}
}
