using SteamInventory.Application.Models;
using SteamInventory.Application.Models.Inventory;

namespace SteamInventory.Application.Services.Steam
{
    public interface ISteamService
    {
        Task<List<WaxpeerAsset>> GetInventoryAsync(long steamId, SteamGame game, CancellationToken cancellationToken = default);
    }
}
