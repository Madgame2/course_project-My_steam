using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using My_steam_server.Interfaces;

namespace My_steam_server.Repositories
{
    public class GamesStaticFilesRepository : IGamesStaticFilesRepository
    {
        private readonly string _gamesDirectory;

        public GamesStaticFilesRepository()
        {
            _gamesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "resoures", "gamesResources");
            
            // Ensure directory exists
            if (!Directory.Exists(_gamesDirectory))
            {
                Directory.CreateDirectory(_gamesDirectory);
            }
        }

        public async Task<Stream?> GetGameFileAsync(int gameId)
        {
            try
            {
                var filePath = GetGameFilePath(gameId);
                if (!File.Exists(filePath))
                {
                    return null;
                }

                return File.OpenRead(filePath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<bool> GameFileExistsAsync(int gameId)
        {
            var filePath = GetGameFilePath(gameId);
            return Task.FromResult(File.Exists(filePath));
        }

        public Task<long?> GetGameFileSizeAsync(int gameId)
        {
            try
            {
                var filePath = GetGameFilePath(gameId);
                if (!File.Exists(filePath))
                {
                    return Task.FromResult<long?>(null);
                }

                var fileInfo = new FileInfo(filePath);
                return Task.FromResult<long?>(fileInfo.Length);
            }
            catch (Exception ex)
            {
                return Task.FromResult<long?>(null);
            }
        }

        private string GetGameFilePath(int gameId)
        {
            return Path.Combine(_gamesDirectory, $"game_{gameId}.zip");
        }
    }
} 