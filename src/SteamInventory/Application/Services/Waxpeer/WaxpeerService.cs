using System.Text;
using Casino.SharedLibrary.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SteamInventory.Application.Services.Waxpeer.Models;

namespace SteamInventory.Application.Services.Waxpeer
{
	public sealed class WaxpeerService : IWaxpeerService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly string _getUserInfoUrl;
		private readonly string _getTradeLinkInfoUrl;

		public WaxpeerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			var apiKey = configuration["Waxpeer:ApiKey"];
			var merchantName = configuration["Waxpeer:MerchantName"];

			_getUserInfoUrl = string.Format(configuration["Waxpeer:Endpoints:GetUserInfo"], apiKey, merchantName);
			_getTradeLinkInfoUrl = string.Format(configuration["Waxpeer:Endpoints:GetTradeLinkInfo"], apiKey);
		}

		public async Task<UserInfo?> GetUserInfoAsync(long steamId)
		{
			var requestUrl = string.Format(_getUserInfoUrl, steamId);
			var jsonObject = await HttpUtils.Get(_httpClientFactory, requestUrl);

			var isSucceeded = jsonObject.Value<bool>("success");
			if (!isSucceeded)
				return null;

			var usersInfo = new UserInfo
			{
				SteamId = steamId.ToString(),
				TradeLink = jsonObject["user"].Value<string>("tradelink"),
				CanSell = jsonObject["user"].Value<bool>("can_sell"),
				CanP2P = jsonObject["user"].Value<bool>("can_p2p")
			};

			return usersInfo;
		}

		public async Task<TradeLinkInfo> GetTradeLinkInfoAsync(string tradeLink)
		{
			var contentData = new
			{
				tradelink = tradeLink
			};

			var serializedData = JsonConvert.SerializeObject(contentData);
			var requestContent = new StringContent(serializedData, Encoding.UTF8, HttpUtils.JsonMediaType);
			var jsonObject = await HttpUtils.Post(_httpClientFactory, _getTradeLinkInfoUrl, requestContent);

			var tradeLinkInfo = new TradeLinkInfo
			{
				Success = jsonObject.Value<bool>("success"),
				Link = tradeLink,
				Message = jsonObject.Value<string>("msg"),
				SteamId = jsonObject.Value<string>("steamid64"),
				Info = jsonObject.Value<string>("info"),
				Token = jsonObject.Value<string>("token")
			};

			return tradeLinkInfo;
		}
	}
}
