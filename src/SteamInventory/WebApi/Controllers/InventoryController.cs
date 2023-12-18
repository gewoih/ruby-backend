using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SteamInventory.Application.Models;
using SteamInventory.Application.Services;

namespace SteamInventory.WebApi.Controllers
{
	[Route("api")]
	public class InventoryController : Controller
	{
		private readonly ISteamService _steamService;

		public InventoryController(ISteamService steamService)
		{
			_steamService = steamService;
		}

		[HttpGet("inventory/{steamId}/{steamGame}")]
		public async Task<IActionResult> Get([Required] long steamId, [Required] SteamGame steamGame, CancellationToken cancellationToken)
		{
			var inventory = await _steamService.GetInventoryAsync(steamId, steamGame, cancellationToken);
			return Ok(inventory);
		}
	}
}
