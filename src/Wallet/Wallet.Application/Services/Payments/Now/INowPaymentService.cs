using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Models;
using Wallet.Domain.Models.Payments.NowPayments;
using Wallet.Infrastructure.Models.NowPayments;

namespace Wallet.Application.Services.Payments.Now
{
	public interface INowPaymentService
	{
		Task<NowPayment> CreatePaymentAsync(Guid userId, CreatePaymentRequestDto createPaymentRequest);
		Task<IEnumerable<CurrencyInfoDto>> GetCurrenciesAsync();
	}
}
