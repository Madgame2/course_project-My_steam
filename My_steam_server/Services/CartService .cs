using Game_Net_DTOLib;
using My_steam_server.Interfaces;

namespace My_steam_server.Services
{
    public class CartService:ICartService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPurchaseOptionRepository _purchaseOptionRepository;

        public CartService(IUserRepository userRepository, IPurchaseOptionRepository purchaseOptionRepository)
        {
            _userRepository = userRepository;
            _purchaseOptionRepository = purchaseOptionRepository;
        }

        public async Task<NetResponse<bool>> AddToCart(long usserId, long purchaseId)
        {
            var user = await _userRepository.GetByIdAsync(usserId);
            if (user == null) return new NetResponse<bool> { Success = false, data = false, resultCode = ResultCode.UnKnowError };

            var pusrchause = await _purchaseOptionRepository.GetByIdAsync(purchaseId);
            if (pusrchause == null) return new NetResponse<bool> { Success = false, data = false, resultCode = ResultCode.UnKnowError };

            try
            {
                await _userRepository.AddToCartAsync(usserId, pusrchause);

                return new NetResponse<bool> { Success = true, data = true };
            }
            catch (ArgumentException)
            {
                return new NetResponse<bool> { Success = false, data = false, resultCode = ResultCode.PurchouseAlredyExist };

            }
        }

        public async Task<NetResponse<List<Game_Net_DTOLib.CartItemDto>?>> getUserCart(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return new NetResponse<List<Game_Net_DTOLib.CartItemDto>?> { Success = false, data = default, resultCode = ResultCode.UnKnowError };

            var output = new List<Game_Net_DTOLib.CartItemDto>();
            foreach (var item in user.CartItems)
            {
                var newItem = new Game_Net_DTOLib.CartItemDto();
                newItem.CarItemtId = item.CartItemId;

                if (item.PurchaseOption == null)
                {
                    var purchouseOption = await _purchaseOptionRepository.GetByIdAsync(item.PurchaseOptionId);
                    if (purchouseOption == null) throw new Exception("cant find this Purchase option");

                    newItem.purchouseNmae = purchouseOption.PurchaseName;
                    newItem.Price = purchouseOption.Price;
                    newItem.ImageLink = purchouseOption.ImageLink;

                }
                else
                {
                    newItem.purchouseNmae = item.PurchaseOption.PurchaseName;
                    newItem.Price = item.PurchaseOption.Price;
                    newItem.ImageLink = item.PurchaseOption.ImageLink;
                }

                output.Add(newItem);
            }

            return new NetResponse<List<Game_Net_DTOLib.CartItemDto>?> { Success = true, data = output };
        }
    }
}
