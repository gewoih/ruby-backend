using System.ComponentModel.DataAnnotations;
using Casino.SharedLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using SteamInventory.Application.Models;
using SteamInventory.Application.Models.Inventory;
using SteamInventory.Application.Services.Steam;
using SteamInventory.Application.Services.Waxpeer;

namespace SteamInventory.WebApi.Controllers
{
    [Route("api")]
	public class InventoryController : Controller
	{
		private readonly ISteamService _steamService;
		private readonly IWaxpeerService _waxpeerService;

		public InventoryController(ISteamService steamService, IWaxpeerService waxpeerService)
		{
			_steamService = steamService;
			_waxpeerService = waxpeerService;
		}

		[HttpGet("inventory/v1/{steamId}/{steamGame}")]
		public async Task<IActionResult> GetInventory([Required] long steamId, [Required] SteamGame steamGame, CancellationToken cancellationToken)
		{
			var inventory = await _steamService.GetInventoryAsync(steamId, steamGame, cancellationToken);
			var response = ApiResponse<List<WaxpeerAsset>>.Success(inventory);
			return Ok(response);
		}

		[HttpGet("inventory/v2/{steamId}/{steamGame}")]
		public async Task<IActionResult> GetInventoryV2([Required] long steamId, [Required] string tradeLink, [Required] SteamGame steamGame, 
			CancellationToken cancellationToken)
		{
			var waxpeerUserInfo = await _waxpeerService.GetUserInfoAsync(steamId);
			if (waxpeerUserInfo is null)
				waxpeerUserInfo = await _waxpeerService.AddUserAsync(steamId, tradeLink);

			if (waxpeerUserInfo is null)
			{
				var response = ApiResponse<List<WaxpeerAsset>>.Error("Невозможно получить инвентарь пользователя: указан некорректный SteamID");
				return BadRequest(response);
			}

			var steamAssets = await _waxpeerService.GetSteamAssetsAsync(steamId, steamGame);
			var inventoryResponse = ApiResponse<List<WaxpeerAsset>>.Success(steamAssets);
			return Ok(inventoryResponse);
		}
	}
}
