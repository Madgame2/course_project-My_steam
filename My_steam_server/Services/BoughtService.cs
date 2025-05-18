using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Linq;

namespace My_steam_server.Services
{
    public class BoughtService : IBoughtService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseOptionRepository _purchaseOptionRepository;
        private readonly IGoodRepository _goodsRepository;
        private readonly IUserLibraryRepository _userLibraryRepository;


        public BoughtService(IUserRepository userRepository, IPurchaseOptionRepository purchaseOptionRepository, IGoodRepository goodsRepository, IUserLibraryRepository userLibraryRepository)
        {
            _userRepository = userRepository;
            _purchaseOptionRepository = purchaseOptionRepository;
            _goodsRepository = goodsRepository;
            _userLibraryRepository = userLibraryRepository;
        }

        public async Task<bool> buyUserCart(string userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                // Получаем корзину и делаем копию, чтобы избежать ошибки изменения коллекции во время перебора
                var userCarts = await _userRepository.GetCartAsync(userId);
                var userCartsCopy = userCarts.ToList(); // Копия списка

                foreach (var cart in userCartsCopy)
                {
                    var purchaseOption = cart.PurchaseOption;
                    if (purchaseOption == null)
                        purchaseOption = await _purchaseOptionRepository.GetByIdAsync(cart.PurchaseOptionId);

                    if (purchaseOption == null) continue;

                    foreach (var goodReceived in purchaseOption.GoodsReceived)
                    {
                        var game = await _goodsRepository.GetByIdAsync(goodReceived.GoodId);
                        if (game == null) continue;

                        if (await _userLibraryRepository.IsGameInLibraryAsync(userId, game.Id)) continue;

                        var entry = new UserLibraryEntry
                        {
                            GameId = game.Id,
                            UserId = userId,
                            PurchaseDate = DateTime.UtcNow
                        };

                        await _userLibraryRepository.AddToLibraryAsync(entry);
                    }

                    await _userRepository.RemoveFromCartAsync(userId, cart.PurchaseOptionId);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[buyUserCart ERROR] {ex}");
                return false;
            }
        }

    }
}
