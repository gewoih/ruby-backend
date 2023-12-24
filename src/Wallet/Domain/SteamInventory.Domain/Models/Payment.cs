using Wallet.Domain.Enums;

namespace Wallet.Domain.Models
{
    public abstract class Payment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
    }
}