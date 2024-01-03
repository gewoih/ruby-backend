using Casino.SharedLibrary.Enums;

namespace Wallet.Domain.Models.Payments.Waxpeer
{
    public class InventoryAsset
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public long AssetId { get; set; }
        public SteamGame SteamGame { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
