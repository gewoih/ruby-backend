using Wallet.Infrastructure.Models.NowPayments;

namespace Wallet.Infrastructure.Services.NowPayments
{
    public interface INowPaymentsService
    {
        Task<IEnumerable<CurrencyInfo>> GetEnabledCurrencies();
    }
}