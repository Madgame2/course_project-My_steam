using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken token);
        Task RevokeAsync(RefreshToken token, string? replacedByToken = null, string? revokedByIp = null);
        Task SaveChangesAsync();
    }
}
