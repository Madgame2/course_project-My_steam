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
        private readonly ILogger<GamesStaticFilesRepository> _logger;

        public GamesStaticFilesRepository(ILogger<GamesStaticFilesRepository> logger)
        {
            _logger = logger;
            _gamesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resoures", "gamesResources");
            
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
                    _logger.LogWarning("Game file not found for game ID: {GameId}", gameId);
                    return null;
                }

                return File.OpenRead(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting game file for game ID: {GameId}", gameId);
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
                _logger.LogError(ex, "Error getting file size for game ID: {GameId}", gameId);
                return Task.FromResult<long?>(null);
            }
        }

        private string GetGameFilePath(int gameId)
        {
            return Path.Combine(_gamesDirectory, $"game_{gameId}.zip");
        }
    }
} 