namespace Wallet.Domain.Models.Payments.NowPayments
{
	public sealed class NowPayment : Payment
	{
		public string PaymentId { get; set; }
		public string PayAddress { get; set; }
		public string PriceCurrency { get; set; }
		public string PayCurrency { get; set; }
		public decimal PayAmount { get; set; }
		public string CallbackUrl { get; set; }
	}
}
