using Microsoft.AspNetCore.Mvc;
using Transactions.Application.Services.Transactions;

namespace Transactions.WebApi.Controllers
{
    [Route("api/transactions")]
    public class TransactionsController : Controller
    {
        private readonly IBalanceTransactionsService _balanceTransactionsService;

        public TransactionsController(IBalanceTransactionsService balanceTransactionsService)
        {
            _balanceTransactionsService = balanceTransactionsService;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> UserBalance(Guid userId)
        {
            var balance = await _balanceTransactionsService.GetUserBalance(userId);
            return Ok(balance);
        }
    }
}