using SteamInventory.Application.Models;
using SteamInventory.Application.Models.Inventory;

namespace SteamInventory.Application.Services
{
    public interface ISteamService
    {
	    Task<List<CounterStrikeAsset>> GetInventoryAsync(long steamId, SteamGame game, CancellationToken cancellationToken = default);
    }
}
