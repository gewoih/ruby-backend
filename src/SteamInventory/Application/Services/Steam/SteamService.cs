using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SteamInventory.Application.Models;
using SteamInventory.Application.Models.Inventory;

namespace SteamInventory.Application.Services.Steam
{
    public class SteamService : ISteamService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _inventoryRequestUrl;

        public SteamService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _inventoryRequestUrl = configuration["ExternalApis:SteamWebApi"];
        }

        public async Task<List<InventoryAsset>> GetInventoryAsync(long steamId, SteamGame game, CancellationToken cancellationToken = default)
        {
            var jsonInventory = await GetJsonInventoryAsync(steamId, game, cancellationToken);
            var jsonAssets = JArray.Parse(jsonInventory);

            var steamAssets = new List<InventoryAsset>();

            foreach (var jsonAsset in jsonAssets)
            {
                var steamAsset = new InventoryAsset
                {
                    SteamUserId = steamId,
                    SteamGame = game,
                    AssetId = jsonAsset.Value<long>("assetid"),
                    MarketName = jsonAsset.Value<string>("markethashname"),
                    Marketable = jsonAsset.Value<bool>("marketable"),
                    ImageUrl = jsonAsset.Value<string>("image"),
                    MarketPrice = jsonAsset.Value<double>("pricesafe")
                };

                steamAssets.Add(steamAsset);
            }

            return steamAssets;
        }

        private async Task<string> GetJsonInventoryAsync(long steamId, SteamGame game, CancellationToken cancellationToken = default)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var gameName = "csgo";
            if (game == SteamGame.Dota)
                gameName = "dota";

            var requestUrl = string.Format(_inventoryRequestUrl, steamId, gameName);
            var response = await httpClient.GetAsync(requestUrl, cancellationToken);

            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return jsonContent;
        }
    }
}
