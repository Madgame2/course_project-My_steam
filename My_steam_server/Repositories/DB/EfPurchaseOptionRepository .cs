using My_steam_server.Interfaces;
using My_steam_server.Models;
using Microsoft.EntityFrameworkCore;

using System;

namespace My_steam_server.Repositories.DB
{
    public class EfPurchaseOptionRepository : IPurchaseOptionRepository
    {

        private readonly ApplicationDbContext _context;

        public EfPurchaseOptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseOption>> GetAllAsync()
        {
            return await _context.PurchaseOptions
                .Include(po => po.GoodsReceived)
                .Include(po => po.Game)
                .ToListAsync();
        }

        public async Task<PurchaseOption?> GetByIdAsync(long id)
        {
            return await _context.PurchaseOptions
                .Include(po => po.GoodsReceived)
                .Include(po => po.Game)
                .FirstOrDefaultAsync(po => po.OptionId == id);
        }

        public async Task AddAsync(PurchaseOption option)
        {
            await _context.PurchaseOptions.AddAsync(option);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PurchaseOption option)
        {
            _context.PurchaseOptions.Update(option);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var option = await _context.PurchaseOptions.FindAsync(id);
            if (option != null)
            {
                _context.PurchaseOptions.Remove(option);
                await _context.SaveChangesAsync();
            }
        }
    }
}
