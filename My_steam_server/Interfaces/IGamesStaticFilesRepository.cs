using System.Threading.Tasks;

namespace My_steam_server.Repositories
{
    public interface IGamesStaticFilesRepository
    {
        /// <summary>
        /// Gets the game file stream by game ID
        /// </summary>
        /// <param name="gameId">The ID of the game</param>
        /// <returns>Stream of the game file if exists, null otherwise</returns>
        Task<Stream?> GetGameFileAsync(int gameId);

        /// <summary>
        /// Checks if game file exists
        /// </summary>
        /// <param name="gameId">The ID of the game</param>
        /// <returns>True if file exists, false otherwise</returns>
        Task<bool> GameFileExistsAsync(int gameId);

        /// <summary>
        /// Gets the size of the game file in bytes
        /// </summary>
        /// <param name="gameId">The ID of the game</param>
        /// <returns>Size of the file in bytes if exists, null otherwise</returns>
        Task<long?> GetGameFileSizeAsync(int gameId);
        Task SaveNewFile(MemoryStream stream ,int gameId);
    }
} 