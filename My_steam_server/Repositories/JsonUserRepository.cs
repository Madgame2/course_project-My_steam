using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Text.Json;

namespace My_steam_server.Repositories
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _filePath = "";
        private List<User> _users;

        public JsonUserRepository(string filePath)
        {
            _filePath = filePath;

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                _users = new List<User>();
            }
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task<User?> GetByIdAsync(long id)
        {
            var user = _users.FirstOrDefault(u => Convert.ToInt32(u.Id) == id);
            return Task.FromResult(user);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task AddUserAsync(User user)
        {
            user.Id = Convert.ToString(_users.Count > 0 ? _users.Max(u => Convert.ToInt32(u.Id)) + 1 : 1);
            _users.Add(user);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task AddToCartAsync(long userId, PurchaseOption purchaseOption)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId.ToString());
            if (user != null)
            {
                var cartItem = user.CartItems.FirstOrDefault(item => item.PurchaseOptionId == purchaseOption.OptionId);
                if (cartItem != null)
                {
                    throw new ArgumentException("Товар уже в корзине.");
                }

                var newId = user.CartItems.Count > 0 ? user.CartItems.Max(c => c.CartItemId) + 1 : 1;
                user.CartItems.Add(new CartItem
                {
                    CartItemId = newId,
                    UserId = userId,
                    PurchaseOptionId = purchaseOption.OptionId
                });

                await SaveChangesAsync();
            }
        }


        public async Task RemoveFromCartAsync(long userId,long purchaseOptionId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId.ToString());
            if (user != null)
            {
                var cartItem = user.CartItems.FirstOrDefault(item => item.PurchaseOptionId == purchaseOptionId);
                if (cartItem != null)
                {
                    user.CartItems.Remove(cartItem);
                    await SaveChangesAsync();
                }
            }
        }

        public Task<List<CartItem>> GetCartAsync(long userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId.ToString());
            if (user != null)
            {
                return Task.FromResult(user.CartItems);
            }
            return Task.FromResult(new List<CartItem>());
        }
    }

}
