namespace Wallet.Application.Models.Waxpeer
{
    public sealed class WaxpeerInventoryAsset
    {
        public long ItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Marketable { get; set; }
        public string ImageUrl { get; set; }
    }
}