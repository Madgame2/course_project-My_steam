using Game_Net_DTOLib;

namespace My_steam_server.Interfaces
{
    public interface ICartService
    {
        Task<NetResponse<bool>> AddToCart(string usserId, long purchaseId);

        Task<NetResponse<List<Game_Net_DTOLib.CartItemDto>?>> getUserCart(string userId);

        Task<NetResponse<bool>> deleteCartElem(string UserID, long CartId);
    }
}
