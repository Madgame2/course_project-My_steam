using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Text.Json;

namespace My_steam_server.Repositories
{
    public class JsonRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _serializerOptions;

        public JsonRefreshTokenRepository(string filePath = "refreshTokens.json")
        {
            _filePath = filePath;
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Если файла нет — создаём пустой список
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }
        public async Task RemoveAsync(string token)
        {
            var tokens = await LoadTokensAsync();
            var toRemove = tokens.FirstOrDefault(rt => rt.Token == token);
            if (toRemove is not null)
            {
                tokens.Remove(toRemove);
                await SaveTokensAsync(tokens);
            }
        }

        public async Task AddAsync(RefreshToken token)
        {
            var tokens = await LoadTokensAsync();
            tokens.Add(token);
            await SaveTokensAsync(tokens);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var tokens = await LoadTokensAsync();
            return tokens.FirstOrDefault(rt => rt.Token == token);
        }

        public async Task RevokeAsync(RefreshToken token, string? replacedByToken = null, string? revokedByIp = null)
        {
            var tokens = await LoadTokensAsync();
            var existing = tokens.FirstOrDefault(rt => rt.Token == token.Token);
            if (existing is null) return;

            existing.Revoked = DateTime.UtcNow;
            existing.RevokedByIp = revokedByIp;
            existing.ReplacedByToken = replacedByToken;
            await SaveTokensAsync(tokens);
        }

        private async Task<List<RefreshToken>> LoadTokensAsync()
        {
            using var stream = File.OpenRead(_filePath);
            return await JsonSerializer.DeserializeAsync<List<RefreshToken>>(stream, _serializerOptions)
                   ?? new List<RefreshToken>();
        }

        private async Task SaveTokensAsync(List<RefreshToken> tokens)
        {
            using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, tokens, _serializerOptions);
        }

        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask;
        }
    }
}
