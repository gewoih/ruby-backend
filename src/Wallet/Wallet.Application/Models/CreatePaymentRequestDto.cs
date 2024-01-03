namespace Wallet.Application.Models
{
	public sealed class CreatePaymentRequestDto
	{
		public decimal PriceAmount { get; set; }
		public string PriceCurrency { get; set; }
		public string PayCurrency { get; set; }
	}
}
