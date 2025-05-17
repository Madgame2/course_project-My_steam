using My_steam_server.Interfaces;
using System.IO.Compression;

namespace My_steam_server.Repositories
{
    public class GamesRepository : IGamesRespository
    {
        private string basePath = Path.Combine(Directory.GetCurrentDirectory(), "GamesFiles");
        public Task<bool> GameExistsAsync(int gameId)
        {
            var filePath = Path.Combine(basePath, $"game_{gameId}.zip");

            return Task.FromResult(File.Exists(filePath));
        }

        public Task<string> GetGameFilePathAsync(int gameId)
        {
            return Task.FromResult(Path.Combine(basePath, $"game_{gameId}.zip"));
        }

        public string getRepositoryPath()
        {
            return basePath;
        }

        public async Task<long> GetUncompressedSize(long gameId)
        {
            return await Task.Run(() =>
            {
                var filePath = Path.Combine(basePath, $"game_{gameId}.zip");
                long totalSize = 0;

                using (ZipArchive archive = ZipFile.OpenRead(filePath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!string.IsNullOrEmpty(entry.Name))
                        {
                            totalSize += entry.Length;
                        }
                    }
                }

                return totalSize;
            });
        }
    }
}
