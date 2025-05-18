using Game_Net_DTOLib;
using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IGoodsService
    {
        Task<NetResponse<Game?>> GetGoodByIdAsync(int goodId); 
        Task<NetResponse<bool>> addGoodAsync(Game good);

        Task<NetResponse<List<Game>>> GetAll();
        Task<NetResponse<List<Game>>> GetProductsAsync(ProductFilterDto filters);
    }
}
