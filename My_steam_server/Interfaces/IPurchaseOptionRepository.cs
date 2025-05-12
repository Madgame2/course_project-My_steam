using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IPurchaseOptionRepository
    {
        Task<PurchaseOption?> GetByIdAsync(long id);
        Task<List<PurchaseOption>> GetAllAsync();
        Task AddAsync(PurchaseOption option);
        Task UpdateAsync(PurchaseOption option);
        Task DeleteAsync(long id);
    }
}
