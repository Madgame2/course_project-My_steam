using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Text.Json;

namespace My_steam_server.Repositories
{
    public class JsonLibRepository : IUserLibraryRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true
        };

        public JsonLibRepository(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        private async Task<List<UserLibraryEntry>> ReadAllAsync()
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<UserLibraryEntry>>(json) ?? new List<UserLibraryEntry>();
        }

        private async Task WriteAllAsync(List<UserLibraryEntry> entries)
        {
            var json = JsonSerializer.Serialize(entries, _jsonOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task AddToLibraryAsync(UserLibraryEntry entry)
        {
            var entries = await ReadAllAsync();
            entries.Add(entry);
            await WriteAllAsync(entries);
        }

        public async Task<List<UserLibraryEntry>> GetLibraryByUserIdAsync(string userId)
        {
            var entries = await ReadAllAsync();
            return entries.Where(e => e.UserId == userId).ToList();
        }

        public async Task<bool> IsGameInLibraryAsync(string userId, long gameId)
        {
            var entries = await ReadAllAsync();
            return entries.Any(e => e.UserId == userId && e.GameId == gameId);
        }
    }
}
