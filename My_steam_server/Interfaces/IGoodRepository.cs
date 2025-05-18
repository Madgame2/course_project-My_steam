using Game_Net_DTOLib;
using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IGoodRepository
    {
        Task<List<Game>> GetAll();
        Task<Game?> GetByIdAsync(long id);
        Task<bool> addAsync(Game entity);

        Task<bool> UpdateAsync(Game entity);
        Task<bool> HasObject(Game entity);

        Task<List<Game>> GetPagesAsync(ProductFilterDto filter);

        Task<Game> CreateEmptyModel(string UssrId);

        Task<List<Game>> GetGamesByUserIdAsync(string userId);

        Task<bool> DeleteAsync(long id);

        Task<bool> DeleteScreenshotsByGameIdAsync(long gameId);
    }
}
