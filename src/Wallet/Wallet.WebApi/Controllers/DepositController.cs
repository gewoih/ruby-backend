using Casino.SharedLibrary.Attributes;
using Casino.SharedLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wallet.Application.Models;
using Wallet.Application.Services.Payments.Now;
using Wallet.Domain.Models.Payments.NowPayments;
using Wallet.Infrastructure.Models.NowPayments;

namespace Wallet.WebApi.Controllers
{
	[Route("api/deposit")]
	public class DepositController : Controller
    {
        private readonly INowPaymentService _nowPaymentsService;

        public DepositController(INowPaymentService nowPaymentsService)
        {
            _nowPaymentsService = nowPaymentsService;
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> Currencies()
        {
            var currencies = await _nowPaymentsService.GetCurrenciesAsync();
            var response = new ApiResponse<IEnumerable<CurrencyInfoDto>>();

            return Ok(response.Success(currencies));
        }

        [HttpPost("payment")]
        public async Task<IActionResult> Payment(
            [NotEmpty] Guid userId, 
            [Required, FromBody] CreatePaymentRequestDto createPaymentRequest)
        {
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var payment = await _nowPaymentsService.CreatePaymentAsync(userId, createPaymentRequest);
            var response = new ApiResponse<NowPayment>().Success(payment);

            return Ok(response);
        }
	}
}
