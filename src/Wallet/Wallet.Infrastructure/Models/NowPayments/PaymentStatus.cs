using Newtonsoft.Json;

namespace Wallet.Infrastructure.Models.NowPayments
{
	public enum PaymentStatus
	{
		Waiting,
		Confirming,
		Confirmed,
		Sending,
		[JsonProperty("partially_paid")]
		PartiallyPaid,
		Finished,
		Failed,
		Refunded,
		Expired
	}
}
