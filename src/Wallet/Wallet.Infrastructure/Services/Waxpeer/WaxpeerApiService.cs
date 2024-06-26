﻿using System.Text;
using Casino.SharedLibrary.Enums;
using Casino.SharedLibrary.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wallet.Infrastructure.Models.Waxpeer;

namespace Wallet.Infrastructure.Services.Waxpeer
{
	public sealed class WaxpeerService : IWaxpeerApiService
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
            
            var httpClient = _httpClientFactory.CreateClient();
            var responseContent = await HttpUtils.GetAsync(httpClient, requestUrl);
            
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

            var httpClient = _httpClientFactory.CreateClient();
            var responseContent = await HttpUtils.PostAsync(httpClient, _addUserUrl, requestContent);

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

            var httpClient = _httpClientFactory.CreateClient();
            var responseContent = await HttpUtils.GetAsync(httpClient, requestUrl);

            return JsonConvert.DeserializeObject<InventoryInfo>(responseContent);
        }

        public async Task<List<WaxpeerInventoryAsset>> GetSteamAssetsAsync(long steamId, SteamGame game)
        {
            var requestUrl = string.Format(_getInventoryAssetsUrl, steamId, (int)game);

            var httpClient = _httpClientFactory.CreateClient();
            var responseContent = await HttpUtils.GetAsync(httpClient, requestUrl);
            var jsonObject = JObject.Parse(responseContent);

            var assets = new List<WaxpeerInventoryAsset>();
            if (!jsonObject.Value<bool>("success"))
                return assets;

            var items = jsonObject.Value<JArray>("items");
            foreach (var item in items)
            {
                var asset = new WaxpeerInventoryAsset
                {
                    ItemId = item.Value<long>("item_id"),
                    Name = item.Value<string>("name"),
                    Marketable = !item.Value<bool>("limit") && item.Value<int>("instant_price") > 0,
                    Price = item.Value<int>("instant_price"),
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

            var httpClient = _httpClientFactory.CreateClient();
            var responseContent = await HttpUtils.PostAsync(httpClient, _getTradeLinkInfoUrl, requestContent);

            var tradeLinkInfo = JsonConvert.DeserializeObject<TradeLinkInfo>(responseContent);

            return tradeLinkInfo;
        }

        public async Task<List<SellItemDto>> SellItemsAsync(long steamId, List<SellItemDto> items)
        {
            var requestUrl = string.Format(_sellItemsUrl, steamId);
            var contentData = new
            {
                items
            };

            var serializedData = JsonConvert.SerializeObject(contentData);
            var requestContent = new StringContent(serializedData, Encoding.UTF8, HttpUtils.JsonMediaType);

            var httpClient = _httpClientFactory.CreateClient();
            var responseContent = await HttpUtils.PostAsync(httpClient, requestUrl, requestContent);

            var listedItems = new List<SellItemDto>();
            var jsonResponse = JObject.Parse(responseContent);
            if (!jsonResponse.Value<bool>("success"))
                return listedItems;

            listedItems = JsonConvert.DeserializeObject<List<SellItemDto>>(jsonResponse["listed"].ToString());
            return listedItems;
        }
    }
}
