using Game_Net_DTOLib;
using Microsoft.EntityFrameworkCore;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using System;

namespace My_steam_server.Repositories.DB
{
    public class EfGoodRepository<T> : IGoodRepository<T> where T : Good, new()
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfGoodRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>(); // DbSet<Game> если T=Game
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> addAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> HasObject(T entity)
        {
            // Проверка на равенство может быть кастомной, здесь сравниваем по Id
            return await _dbSet.AnyAsync(e => e.Id == entity.Id);
        }

        public async Task<List<T>> GetPagesAsync(ProductFilterDto filter)
        {
            IQueryable<T> query = _dbSet.OrderBy(x => x.Id);

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

        public async Task<T> CreateEmptyModel()
        {
            long newId = 0;

            if (await _dbSet.AnyAsync())
            {
                newId = await _dbSet.MaxAsync(e => e.Id) + 1;
            }

            var newEntity = new T
            {
                Id = newId
            };

            return newEntity;
        }
    }

}
