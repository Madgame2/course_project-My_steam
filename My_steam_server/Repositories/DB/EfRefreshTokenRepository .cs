using Microsoft.EntityFrameworkCore;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using System;

namespace My_steam_server.Repositories.DB
{
    public class EfRefreshTokenRepository : IRefreshTokenRepository
    {

        private readonly ApplicationDbContext _context;

        public EfRefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAsync(RefreshToken token, string? replacedByToken = null, string? revokedByIp = null)
        {
            var existing = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token.Token);
            if (existing == null)
                return;

            existing.Revoked = DateTime.UtcNow;
            existing.RevokedByIp = revokedByIp;
            existing.ReplacedByToken = replacedByToken;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string token)
        {
            var existing = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
            if (existing == null)
                return;

            _context.RefreshTokens.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
