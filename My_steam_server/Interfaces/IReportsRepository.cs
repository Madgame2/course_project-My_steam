using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IReportsRepository
    {
        Task<IEnumerable<ReportMessageModel>> GetAllAsync();
        Task<IEnumerable<ReportMessageModel>> GetByUserIdAsync(string userId);
        Task AddAsync(ReportMessageModel report);
    }
}
