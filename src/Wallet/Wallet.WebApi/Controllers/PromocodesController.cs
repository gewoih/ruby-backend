using Casino.SharedLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Services.Promocodes;
using Wallet.Domain.Enums;

namespace Wallet.WebApi.Controllers
{
	[Route("api/promocodes")]
	public class PromocodesController : Controller
	{
		private readonly IPromocodesService _promocodesService;

        public PromocodesController(IPromocodesService promocodesService)
        {
            _promocodesService = promocodesService;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate(Guid userId, PromocodeType type, TimeSpan lifeSpan, int amount, bool isRandomCode = true)
        {
            var promocode = await _promocodesService.CreateAsync(userId, type, lifeSpan, amount, isRandomCode);
            return Ok(promocode);
        }

        [HttpPost("activate")]
        public async Task<IActionResult> Activate(Guid userId, string code)
        {
            var response = new ApiResponse<bool>();

            if (userId == Guid.Empty)
                return BadRequest(response.Error("Id пользователя не может быть пустым"));
            if (string.IsNullOrEmpty(code))
                return BadRequest(response.Error("Промокод не может быть пустым"));

            var isActivated = await _promocodesService.ActivateAsync(userId, code);
            if (isActivated)
                response.Success(isActivated);
            else
                response.Error("Неверный промокод или истек срок его действия");

            return Ok(response);
        }
	}
}
