using Microsoft.EntityFrameworkCore;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using System;

namespace My_steam_server.Repositories.DB
{
    public class EfReportsRepository: IReportsRepository
    {
        private readonly ApplicationDbContext _context;

        public EfReportsRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<ReportMessageModel>> GetAllAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<IEnumerable<ReportMessageModel>> GetByUserIdAsync(string userId)
        {
            return await _context.Reports
                .Where(r => r.UserID == userId)
                .ToListAsync();
        }

        public async Task AddAsync(ReportMessageModel report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
        }
    }
}
