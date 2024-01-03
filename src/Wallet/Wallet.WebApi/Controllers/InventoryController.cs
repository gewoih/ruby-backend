using System.ComponentModel.DataAnnotations;
using Casino.SharedLibrary.Attributes;
using Casino.SharedLibrary.Enums;
using Casino.SharedLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Services.Payments.Waxpeer;
using Wallet.Application.Utils;
using Wallet.Domain.Models.Payments.Waxpeer;
using Wallet.Infrastructure.Models.Waxpeer;
using Wallet.Infrastructure.Services.Waxpeer;

namespace Wallet.WebApi.Controllers
{
    [Route("api/inventory")]
	public class InventoryController : Controller
    {
		private readonly IWaxpeerApiService _waxpeerService;
        private readonly IWaxpeerPaymentService _walletService;

		public InventoryController(IWaxpeerApiService waxpeerService, IWaxpeerPaymentService walletService)
		{
            _waxpeerService = waxpeerService;
            _walletService = walletService;
		}

		[HttpGet]
		public async Task<IActionResult> Inventory(
            [Required, NotEmpty] long steamId,
            [Required, NotEmpty] string tradeLink, 
            [Required, NotEmpty] SteamGame steamGame)
		{
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

			var response = new ApiResponse<List<WaxpeerInventoryAsset>>();

			var waxpeerUserInfo = await _waxpeerService.GetUserInfoAsync(steamId) ?? await _waxpeerService.AddUserAsync(steamId, tradeLink);

			if (waxpeerUserInfo is null)
				return BadRequest(response.Error("Невозможно получить инвентарь пользователя: указан некорректный SteamID"));

			var steamAssets = await _waxpeerService.GetSteamAssetsAsync(steamId, steamGame);
			return Ok(response.Success(steamAssets));
		}

		[HttpPost("sell-items")]
		public async Task<IActionResult> SellItems(
            [NotEmpty] Guid userId, 
            [NotEmpty] long steamId, 
            [FromBody, MinLength(1)] List<SellItemDto> itemsToSell)
		{
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

			var response = new ApiResponse<WaxpeerPayment>();

			var itemsListed = await _waxpeerService.SellItemsAsync(steamId, itemsToSell);
			if (itemsListed.Count == 0)
				return BadRequest(response.Error("Произошла ошибка при выставлении предметов на продажу."));

            var inventoryAssets = itemsListed.ToInventoryAssets(userId);
			var waxpeerPayment = await _walletService.CreateWaxpeerPayment(userId, steamId, inventoryAssets);
            
			return Ok(response.Success(waxpeerPayment));
		}

		[HttpPost("sale-confirmation")]
		public async Task<IActionResult> ConfirmSale([FromBody] SoldItemsWebhookDto soldItemsDto)
        {
            var activePayment = await _walletService.GetActivePayment(soldItemsDto.SteamId) 
                                ?? throw new KeyNotFoundException($"Не найден активный платеж для пользователя Steam '{soldItemsDto.SteamId}'");
            
            await _walletService.CompletePayment(activePayment, soldItemsDto);
			return Ok(soldItemsDto);
		}
	}
}
