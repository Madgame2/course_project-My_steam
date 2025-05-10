using My_steam_server.Interfaces;

namespace My_steam_server.Repositories
{
    public class GamesRepository : IGamesRespository
    {
        private string basePath = Path.Combine(Directory.GetCurrentDirectory(), "GamesFiles");
        public Task<bool> GameExistsAsync(int gameId)
        {
            var filePath = Path.Combine(basePath, $"gmae_{gameId}.zip");

            return Task.FromResult(File.Exists(filePath));
        }

        public Task<string> GetGameFilePathAsync(int gameId)
        {
            return Task.FromResult(Path.Combine(basePath, $"gmae_{gameId}.zip"));
        }
    }
}
