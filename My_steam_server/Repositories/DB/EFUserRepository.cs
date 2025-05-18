using Microsoft.EntityFrameworkCore;
using My_steam_server.Interfaces;
using My_steam_server.Models;

namespace My_steam_server.Repositories.DB
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public EFUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(string userId, PurchaseOption purchaseOption)
        {
            var user = await _context.Users
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new ArgumentException("Пользователь не найден.");

            var alreadyInCart = user.CartItems.Any(c => c.PurchaseOptionId == purchaseOption.OptionId);
            if (alreadyInCart)
                throw new ArgumentException("Товар уже в корзине.");

            var cartItem = new CartItem
            {
                UserId = userId,
                PurchaseOptionId = purchaseOption.OptionId
            };

            user.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.CartItems)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.CartItems)
                    .ThenInclude(c => c.PurchaseOption)
                        .ThenInclude(po => po.GoodsReceived)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.CartItems ?? new List<CartItem>();
        }

        public async Task RemoveFromCartAsync(string userId, long purchaseOptionId)
        {
            var user = await _context.Users
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return;

            var item = user.CartItems.FirstOrDefault(c => c.CartItemId == purchaseOptionId);
            if (item != null)
            {
                _context.CartItems.Remove(item); // явно удалить из DbSet<CartItems>
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
