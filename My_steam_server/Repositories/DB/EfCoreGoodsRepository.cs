using Game_Net_DTOLib;
using Microsoft.EntityFrameworkCore;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using System;

namespace My_steam_server.Repositories.DB
{
    public class EfGoodRepository : IGoodRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Game> _dbSet;

        public EfGoodRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Game>(); // DbSet<Game> если T=Game
        }

        public async Task<List<Game>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(long id)
        {
            return await _context.Games
                .Include(g => g.imageSource)
                .Include(g => g.PurchaseOptions)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> addAsync(Game entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Game entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> HasObject(Game entity)
        {
            // Проверка на равенство может быть кастомной, здесь сравниваем по Id
            return await _dbSet.AnyAsync(e => e.Id == entity.Id);
        }

        public async Task<List<Game>> GetPagesAsync(ProductFilterDto filter)
        {
            IQueryable<Game> query = _dbSet.OrderBy(x => x.Id);

            if (filter.LastSeenId.HasValue)
                query = query.Where(x => x.Id > filter.LastSeenId.Value);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{filter.Search}%"));

            if (filter.minPrice.HasValue)
                query = query.Where(x => x.Price >= filter.minPrice.Value);

            if (filter.maxPrice.HasValue)
                query = query.Where(x => x.Price <= filter.maxPrice.Value);

            return await query
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<Game> CreateEmptyModel(string UssrId)
        {
            var newEntity = new Game(); // Id не задаём, БД сгенерирует
            newEntity.UserId = UssrId;

            await _dbSet.AddAsync(newEntity);
            await _context.SaveChangesAsync();

            return newEntity;
        }

        public async Task<List<Game>> GetGamesByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(g => g.UserId == userId)  // здесь нужно подставить реальное поле
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _context.Games.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }

}
