using Microsoft.EntityFrameworkCore;
using My_steam_server.Interfaces;
using My_steam_server.Models;


namespace My_steam_server.Repositories.DB
{
    public class EfUserLibraryRepository : IUserLibraryRepository
    {
        private readonly ApplicationDbContext _context;

        public EfUserLibraryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToLibraryAsync(UserLibraryEntry entry)
        {
            _context.UserLibraryEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserLibraryEntry>> GetLibraryByUserIdAsync(string userId)
        {
            return await _context.UserLibraryEntries
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }
        public async Task<bool> IsGameInLibraryAsync(string userId, long gameId)
        {
            return await _context.UserLibraryEntries
                .AnyAsync(e => e.UserId == userId && e.GameId == gameId);
        }
    }
}
