using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IUserLibraryRepository
    {
        Task<List<UserLibraryEntry>> GetLibraryByUserIdAsync(string userId);
        Task AddToLibraryAsync(UserLibraryEntry entry);
        Task<bool> IsGameInLibraryAsync(string userId, long gameId);
    }
}
