namespace Casino.SharedLibrary.MessageBus.TopUp
{
    public sealed class PaymentMessage
    {
        public Guid UserId { get; set; }
        public Guid PaymentId { get; set; }
        public TopUpType Type { get; set; }
        public decimal Amount { get; set; }
    }
}
