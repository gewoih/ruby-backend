﻿using Newtonsoft.Json;

namespace SteamInventory.Application.Services.Waxpeer.Models
{
	public sealed class InventoryInfo
	{
		[JsonProperty("tradelinkError")]
		public bool TradeLinkError { get; set; }

		[JsonProperty("msg")]
		public string Message { get; set; }

		[JsonProperty("count")]
		public int Count { get; set; }
	}
}
