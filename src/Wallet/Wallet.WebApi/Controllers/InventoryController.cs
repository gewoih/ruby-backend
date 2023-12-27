using System.ComponentModel.DataAnnotations;
using Casino.SharedLibrary.Enums;
using Casino.SharedLibrary.MessageBus.Transactions;
using Casino.SharedLibrary.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Services.Wallet;
using Wallet.Application.Utils;
using Wallet.Domain.Enums;
using Wallet.Domain.Models.Wallet;
using Wallet.Infrastructure.Models.Waxpeer;
using Wallet.Infrastructure.Services.Waxpeer;

namespace Wallet.WebApi.Controllers
{
    [Route("api/inventory")]
	public class InventoryController : Controller
    {
		private readonly IWaxpeerService _waxpeerService;
        private readonly IWalletService _walletService;
		private readonly IBus _messagesBus;

		public InventoryController(IWaxpeerService waxpeerService, IWalletService walletService, IBus messagesBus)
		{
            _waxpeerService = waxpeerService;
            _walletService = walletService;
            _messagesBus = messagesBus;
		}

		[HttpGet]
		public async Task<IActionResult> Inventory([Required] long steamId, [Required] string tradeLink, [Required] SteamGame steamGame)
		{
			var response = new ApiResponse<List<WaxpeerInventoryAsset>>();

			var waxpeerUserInfo = await _waxpeerService.GetUserInfoAsync(steamId) ?? await _waxpeerService.AddUserAsync(steamId, tradeLink);

			if (waxpeerUserInfo is null)
				return BadRequest(response.Error("Невозможно получить инвентарь пользователя: указан некорректный SteamID"));

			var steamAssets = await _waxpeerService.GetSteamAssetsAsync(steamId, steamGame);
			return Ok(response.Success(steamAssets));
		}

		[HttpPost("sell-items")]
		public async Task<IActionResult> SellItems(Guid userId, long steamId, [FromBody] List<SellItemDto> itemsToSell)
		{
			var response = new ApiResponse<WaxpeerPayment>();

            if (userId == Guid.Empty)
                return BadRequest(response.Error("UserId не может быть пустым."));
            if (steamId == 0)
                return BadRequest(response.Error("SteamId не может быть пустым."));
			if (itemsToSell.Count == 0)
				return BadRequest(response.Error("Список предметов для продажи не может быть пустым."));

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

            var paymentMessage = new TransactionTrigger
			{
				Amount = soldItemsDto.Items
					.Where(item => item.Status.Equals(5))
					.Sum(item => item.Price),

				Type = TransactionTriggerType.Payment,
				UserId = activePayment.UserId,
				TriggerId = activePayment.Id
			};
            
			await _messagesBus.Publish(paymentMessage);

            activePayment.Status = PaymentStatus.Completed;
            await _walletService.UpdatePayment(activePayment);

			return Ok(soldItemsDto);
		}
	}
}
