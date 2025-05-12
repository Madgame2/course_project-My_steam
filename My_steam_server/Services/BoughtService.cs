using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Linq;

namespace My_steam_server.Services
{
    public class BoughtService : IBoughtService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseOptionRepository _purchaseOptionRepository;
        private readonly IGoodRepository<Game> _goodsRepository;
        private readonly IUserLibraryRepository _userLibraryRepository;


        public BoughtService(IUserRepository userRepository, IPurchaseOptionRepository purchaseOptionRepository, IGoodRepository<Game> goodsRepository, IUserLibraryRepository userLibraryRepository)
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

                var userCarts = await _userRepository.GetCartAsync(userId);


                foreach (var cart in userCarts)
                {
                    var purchouseOption = cart.PurchaseOption;
                    if (purchouseOption == null) purchouseOption = await _purchaseOptionRepository.GetByIdAsync(cart.PurchaseOptionId);

                    if (purchouseOption != null)
                        foreach (var goodReceived in purchouseOption.GoodsReceived)
                        {
                            var Game = await _goodsRepository.GetByIdAsync(goodReceived.GoodId);
                            if (Game == null) continue;

                            if (await _userLibraryRepository.IsGameInLibraryAsync(userId, Game.Id)) continue;


                            var entry = new UserLibraryEntry
                            {
                                GameId = Game.Id,
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
