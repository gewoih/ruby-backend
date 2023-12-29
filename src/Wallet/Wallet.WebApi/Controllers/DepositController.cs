using Casino.SharedLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Wallet.Infrastructure.Models.NowPayments;
using Wallet.Infrastructure.Services.NowPayments;

namespace Wallet.WebApi.Controllers
{
    [Route("api/deposit")]
	public class DepositController : Controller
    {
        private readonly INowPaymentsService _nowPaymentsService;

        public DepositController(INowPaymentsService nowPaymentsService)
        {
            _nowPaymentsService = nowPaymentsService;
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> Currencies()
        {
            var currencies = await _nowPaymentsService.GetEnabledCurrencies();
            var response = new ApiResponse<IEnumerable<CurrencyInfo>>();

            return Ok(response.Success(currencies));
        }
	}
}
