using Game_Net_DTOLib;

namespace My_steam_server.Interfaces
{
    public interface ICartService
    {
        Task<NetResponse<bool>> AddToCart(long usserId, long purchaseId);

        Task<NetResponse<List<Game_Net_DTOLib.CartItemDto>?>> getUserCart(long userId);
    }
}
