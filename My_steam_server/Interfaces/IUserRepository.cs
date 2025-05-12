//using Game_Net_DTOLib;
using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();


        Task AddToCartAsync(long userId, PurchaseOption purchaseOption);
        Task RemoveFromCartAsync(long userId, long purchaseOptionId);

        Task<List<CartItem>> GetCartAsync(long userId);
    }
}
