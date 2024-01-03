using Newtonsoft.Json;

namespace Wallet.Infrastructure.Models.NowPayments
{
	public sealed class CreatePaymentDto
	{
		[JsonProperty("price_amount")]
		public decimal PriceAmount { get; set; }

		[JsonProperty("price_currency")]
		public string PriceCurrency { get; set; }

		[JsonProperty("pay_currency")]
		public string PayCurrency { get; set; }

		[JsonProperty("ipn_callback_url", NullValueHandling = NullValueHandling.Ignore)]
		public string CallbackUrl { get; set; }

		[JsonProperty("order_id")]
		public string OrderId { get; set; }

		[JsonProperty("order_description", NullValueHandling = NullValueHandling.Ignore)]
		public string Description { get; set; }
	}
}
