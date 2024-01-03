using Wallet.Infrastructure.Models.NowPayments;

namespace Wallet.Infrastructure.Services.NowPayments
{
    public interface INowPaymentsApiService
    {
        Task<IEnumerable<CurrencyInfoDto>> GetEnabledCurrenciesAsync();
        Task<PaymentInfoDto> CreatePaymentAsync(CreatePaymentDto createPaymentDto);
	}
}