using My_steam_server.Models;

namespace My_steam_server.Repositories
{
    public interface IReportsService
    {
        Task<IEnumerable<ReportMessageModel>> GetReportsAsync();
        Task<IEnumerable<ReportMessageModel>> GetReportsByUserAsync(string userId);
        Task AddReportAsync(ReportMessageModel report);
    }
}
