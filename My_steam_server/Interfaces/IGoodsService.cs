using Game_Net_DTOLib;
using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IGoodsService<T> where T : Good
    {
        Task<NetResponse<T?>> GetGoodByIdAsync(int goodId); 
        Task<NetResponse<bool>> addGoodAsync(T good);

        Task<NetResponse<List<T>>> GetAll();
    }
}
