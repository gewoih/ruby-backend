using System.ComponentModel.DataAnnotations;
using Casino.SharedLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using SteamInventory.Application.Models;
using SteamInventory.Application.Models.Inventory;
using SteamInventory.Application.Services.Steam;
using SteamInventory.Application.Services.Waxpeer;
using SteamInventory.Application.Services.Waxpeer.Models;

namespace SteamInventory.WebApi.Controllers
{
    [Route("api/inventory")]
	public class InventoryController : Controller
	{
		private readonly ISteamService _steamService;
		private readonly IWaxpeerService _waxpeerService;

		public InventoryController(ISteamService steamService, IWaxpeerService waxpeerService)
		{
			_steamService = steamService;
			_waxpeerService = waxpeerService;
		}

		[HttpGet("v1/{steamId}/{steamGame}")]
		public async Task<IActionResult> GetInventory([Required] long steamId, [Required] SteamGame steamGame, CancellationToken cancellationToken)
		{
			var response = new ApiResponse<List<InventoryAsset>>();
			var inventory = await _steamService.GetInventoryAsync(steamId, steamGame, cancellationToken);
			return Ok(response.Success(inventory));
		}

		[HttpGet("v2")]
		public async Task<IActionResult> GetInventoryV2([Required] long steamId, [Required] string tradeLink, [Required] SteamGame steamGame, 
			CancellationToken cancellationToken)
		{
			var response = new ApiResponse<List<InventoryAsset>>();

			var waxpeerUserInfo = await _waxpeerService.GetUserInfoAsync(steamId);
			if (waxpeerUserInfo is null)
				waxpeerUserInfo = await _waxpeerService.AddUserAsync(steamId, tradeLink);

			if (waxpeerUserInfo is null)
				return BadRequest(response.Error("Невозможно получить инвентарь пользователя: указан некорректный SteamID"));

			var steamAssets = await _waxpeerService.GetSteamAssetsAsync(steamId, steamGame);
			return Ok(response.Success(steamAssets));
		}

		[HttpPost("items")]
		public async Task<IActionResult> SellItems([Required] string steamId, [FromBody] List<WaxpeerItem> itemsToSell)
		{
			var response = new ApiResponse<List<WaxpeerItem>>();

			if (!itemsToSell.Any())
				return BadRequest(response.Error("Список предметов для продажи не может быть пустым."));

			var itemsListed = await _waxpeerService.SellItemsAsync(steamId, itemsToSell);
			if (!itemsListed.Any())
				return BadRequest(response.Error("Произошла ошибка при выставлении предметов на продажу."));

			return Ok(response.Success(itemsListed));
		}
	}
}
