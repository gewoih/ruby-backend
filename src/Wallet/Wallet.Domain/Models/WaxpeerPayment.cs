namespace Wallet.Domain.Models
{
    public sealed class WaxpeerPayment : Payment
    {
        public long SteamId { get; set; }
        public List<InventoryAsset> Items { get; set; }
    }
}