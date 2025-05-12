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
    }
}
