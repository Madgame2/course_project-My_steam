//using Game_Net_DTOLib;
using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();


        Task AddToCartAsync(string userId, PurchaseOption purchaseOption);
        Task RemoveFromCartAsync(string userId, long purchaseOptionId);

        Task<List<CartItem>> GetCartAsync(string userId);
    }
}
