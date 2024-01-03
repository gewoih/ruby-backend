using Wallet.Application.Models;
using Wallet.Domain.Models.Payments;
using Wallet.Domain.Models.Payments.NowPayments;
using Wallet.Infrastructure.Database;
using Wallet.Infrastructure.Models.NowPayments;
using Wallet.Infrastructure.Services.NowPayments;

namespace Wallet.Application.Services.Payments.Now
{
	public sealed class NowPaymentService : INowPaymentService
	{
        private readonly INowPaymentsApiService _nowPaymentsApiService;
		private readonly WalletDbContext _context;

		public NowPaymentService(INowPaymentsApiService nowPaymentsApiService, WalletDbContext context)
		{
			_nowPaymentsApiService = nowPaymentsApiService;
			_context = context;
		}

		public async Task<IEnumerable<CurrencyInfoDto>> GetCurrenciesAsync()
		{
			var currencies = await _nowPaymentsApiService.GetEnabledCurrenciesAsync();
			return currencies;
		}

		public async Task<NowPayment> CreatePaymentAsync(Guid userId, CreatePaymentRequestDto createPaymentRequest)
		{
			var internalPayment = await CreateInternalPaymentAsync(userId, createPaymentRequest);
			var externalPayment = await CreateExternalPaymentAsync(internalPayment);

			await UpdateInternalPayment(internalPayment, externalPayment);

			return internalPayment;
		}

		private async Task<NowPayment> CreateInternalPaymentAsync(Guid userId, CreatePaymentRequestDto createPaymentRequest)
		{
			var payment = new NowPayment
			{
				Amount = createPaymentRequest.PriceAmount,
				CreatedDate = DateTime.UtcNow,
				Status = Domain.Enums.PaymentStatus.Created,
				UserId = userId,
				PriceCurrency = createPaymentRequest.PriceCurrency,
				PayCurrency = createPaymentRequest.PayCurrency
			};

			await _context.NowPayments.AddAsync(payment);
			await _context.SaveChangesAsync();

			return payment;
		}

		private async Task<PaymentInfoDto> CreateExternalPaymentAsync(NowPayment internalPayment)
		{
			var paymentInfo = await _nowPaymentsApiService.CreatePaymentAsync(new CreatePaymentDto
			{
				PayCurrency = internalPayment.PayCurrency,
				PriceAmount = internalPayment.Amount,
				OrderId = internalPayment.Id.ToString(),
				PriceCurrency = internalPayment.PriceCurrency,
			});

			return paymentInfo;
		}

		private async Task UpdateInternalPayment(NowPayment internalPayment, PaymentInfoDto externalPayment)
		{
			internalPayment.PayAddress = externalPayment.PayAddress;
			internalPayment.PayAmount = externalPayment.PayAmount;
			await _context.SaveChangesAsync();
		}
	}
}
