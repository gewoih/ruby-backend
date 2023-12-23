using System.Text;
using Casino.SharedLibrary.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wallet.Application.Models;
using Wallet.Application.Models.Inventory;
using Wallet.Application.Models.Waxpeer;

namespace Wallet.Application.Services.Waxpeer
{
	public sealed class WaxpeerService : IWaxpeerService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly string _getUserInfoUrl;
		private readonly string _getTradeLinkInfoUrl;
		private readonly string _addUserUrl;
		private readonly string _getInventoryInfoUrl;
		private readonly string _getInventoryAssetsUrl;
		private readonly string _sellItemsUrl;

		public WaxpeerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			var apiKey = configuration["Waxpeer:ApiKey"];
			var merchantName = configuration["Waxpeer:MerchantName"];

			_getUserInfoUrl = string.Format(configuration["Waxpeer:Endpoints:GetUserInfo"], apiKey, merchantName, "{0}");
			_getTradeLinkInfoUrl = string.Format(configuration["Waxpeer:Endpoints:GetTradeLinkInfo"], apiKey);
			_addUserUrl = string.Format(configuration["Waxpeer:Endpoints:AddUser"], apiKey, merchantName);
			_getInventoryInfoUrl = string.Format(configuration["Waxpeer:Endpoints:GetInventoryInfo"], apiKey, merchantName, "{0}");
			_getInventoryAssetsUrl = string.Format(configuration["Waxpeer:Endpoints:GetInventoryAssets"], apiKey, merchantName, "{0}", "{1}");
			_sellItemsUrl = string.Format(configuration["Waxpeer:Endpoints:SellItems"], apiKey, merchantName, "{0}");
		}

		public async Task<UserInfo?> GetUserInfoAsync(long steamId)
		{
			var requestUrl = string.Format(_getUserInfoUrl, steamId);
			var responseContent = await HttpUtils.GetAsync(_httpClientFactory, requestUrl);
			var jsonObject = JObject.Parse(responseContent);
			if (!jsonObject.Value<bool>("success"))
				return null;

			var userContent = jsonObject["user"].ToString();
			var userInfo = JsonConvert.DeserializeObject<UserInfo>(userContent);

			return userInfo;
		}

		public async Task<UserInfo?> AddUserAsync(long steamId, string tradeLink)
		{
			var contentData = new
			{
				tradeLink,
				steamId
			};

			var serializedData = JsonConvert.SerializeObject(contentData);
			var requestContent = new StringContent(serializedData, Encoding.UTF8, HttpUtils.JsonMediaType);
			var responseContent = await HttpUtils.PostAsync(_httpClientFactory, _addUserUrl, requestContent);
			var jsonObject = JObject.Parse(responseContent);
			if (!jsonObject.Value<bool>("success"))
				return null;

			var userContent = jsonObject["user"].ToString();
			var userInfo = JsonConvert.DeserializeObject<UserInfo>(userContent);

			return userInfo;
		}

		public async Task<InventoryInfo?> GetInventoryInfoAsync(long steamId)
		{
			var requestUrl = string.Format(_getInventoryInfoUrl, steamId);
			var responseContent = await HttpUtils.GetAsync(_httpClientFactory, requestUrl);

			return JsonConvert.DeserializeObject<InventoryInfo>(responseContent);
		}

		public async Task<List<InventoryAsset>> GetSteamAssetsAsync(long steamId, SteamGame game)
		{
			var requestUrl = string.Format(_getInventoryAssetsUrl, steamId, (int)game);
			var responseContent = await HttpUtils.GetAsync(_httpClientFactory, requestUrl);
			var jsonObject = JObject.Parse(responseContent);

			var assets = new List<InventoryAsset>();
			if (!jsonObject.Value<bool>("success"))
				return assets;

			var items = jsonObject.Value<JArray>("items");
			foreach (var item in items)
			{
				var asset = new InventoryAsset
				{
					AssetId = item.Value<long>("item_id"),
					MarketName = item.Value<string>("name"),
					Marketable = item.Value<bool>("limit") == false && item.Value<int>("instant_price") > 0,
					MarketPrice = item.Value<int>("instant_price"),
					ImageUrl = item.Value<JObject>("steam_price").Value<string>("img"),
				};

				assets.Add(asset);
			}

			return assets;
		}

		public async Task<TradeLinkInfo?> GetTradeLinkInfoAsync(string tradeLink)
		{
			var contentData = new
			{
				tradelink = tradeLink
			};

			var serializedData = JsonConvert.SerializeObject(contentData);
			var requestContent = new StringContent(serializedData, Encoding.UTF8, HttpUtils.JsonMediaType);
			var responseContent = await HttpUtils.PostAsync(_httpClientFactory, _getTradeLinkInfoUrl, requestContent);

			var tradeLinkInfo = JsonConvert.DeserializeObject<TradeLinkInfo>(responseContent);

			return tradeLinkInfo;
		}

		public async Task<List<WaxpeerItem>> SellItemsAsync(string steamId, List<WaxpeerItem> items)
		{
			var requestUrl = string.Format(_sellItemsUrl, steamId);
			var contentData = new
			{
				items
			};

			var serializedData = JsonConvert.SerializeObject(contentData);
			var requestContent = new StringContent(serializedData, Encoding.UTF8, HttpUtils.JsonMediaType);
			var responseContent = await HttpUtils.PostAsync(_httpClientFactory, requestUrl, requestContent);

			var listedItems = new List<WaxpeerItem>();
			var jsonResponse = JObject.Parse(responseContent);
			if (!jsonResponse.Value<bool>("success"))
				return listedItems;

			listedItems = JsonConvert.DeserializeObject<List<WaxpeerItem>>(jsonResponse["items"].ToString());
			return listedItems;
		}
	}
}
