using Casino.SharedLibrary.Attributes;

namespace Wallet.Application.Models
{
	public sealed class CreatePaymentRequestDto
	{
		[NotEmpty]
		public decimal PriceAmount { get; set; }

		[NotEmpty]
		public string PriceCurrency { get; set; }

		[NotEmpty]
		public string PayCurrency { get; set; }
	}
}
