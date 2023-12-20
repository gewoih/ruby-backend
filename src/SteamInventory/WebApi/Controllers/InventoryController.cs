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

		[HttpGet("inventory/{steamId}/{steamGame}")]
		public async Task<IActionResult> Get([Required] long steamId, [Required] SteamGame steamGame, CancellationToken cancellationToken)
		{
			var inventory = await _steamService.GetInventoryAsync(steamId, steamGame, cancellationToken);
			var response = new ApiResponse<List<CounterStrikeAsset>>().Success(inventory);
			return Ok(response);
		}
	}
}
