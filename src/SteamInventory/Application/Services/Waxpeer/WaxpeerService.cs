﻿using System.Text;
using Casino.SharedLibrary.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamInventory.Application.Models;
using SteamInventory.Application.Models.Inventory;
using SteamInventory.Application.Services.Waxpeer.Models;

namespace SteamInventory.Application.Services.Waxpeer
{
	public sealed class WaxpeerService : IWaxpeerService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly string _getUserInfoUrl;
		private readonly string _getTradeLinkInfoUrl;
		private readonly string _addUserUrl;
		private readonly string _getInventoryInfoUrl;
		private readonly string _getInventoryAssetsUrl;

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

		public async Task<List<WaxpeerAsset>> GetSteamAssetsAsync(long steamId, SteamGame game)
		{
			var requestUrl = string.Format(_getInventoryAssetsUrl, steamId, (int)game);
			var responseContent = await HttpUtils.GetAsync(_httpClientFactory, requestUrl);
			var jsonObject = JObject.Parse(responseContent);

			var assets = new List<WaxpeerAsset>();
			if (!jsonObject.Value<bool>("success"))
				return assets;

			var items = jsonObject.Value<JArray>("items");
			foreach (var item in items)
			{
				var asset = new WaxpeerAsset
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
	}
}
