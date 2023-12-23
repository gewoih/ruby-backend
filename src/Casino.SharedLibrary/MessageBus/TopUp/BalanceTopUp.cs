namespace Casino.SharedLibrary.MessageBus.TopUp
{
    public sealed class BalanceTopUp
    {
        public Guid UserId { get; set; }
        public TopUpType Type { get; set; }
        public decimal Amount { get; set; }
    }
}
