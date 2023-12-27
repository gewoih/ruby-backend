using Wallet.Domain.Enums;

namespace Wallet.Domain.Models
{
	public sealed class Promocode
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public bool IsActivated { get; set; }
		public string Code { get; set; }
		public PromocodeType Type { get; set; }
		public int Amount { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public DateTime? ActivatedDateTime { get; set; }
		public DateTime ValidTillDateTime { get; set; }
	}
}
