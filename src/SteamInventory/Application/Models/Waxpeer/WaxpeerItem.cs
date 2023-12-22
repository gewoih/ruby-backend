using Newtonsoft.Json;

namespace SteamInventory.Application.Models.Waxpeer
{
    public sealed class WaxpeerItem
    {
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("instant")]
        public bool IsInstant { get; set; }
    }
}
