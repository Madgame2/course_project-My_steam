using My_steam_server.Interfaces;
using My_steam_server.Models;
using My_steam_server.Repositories;

namespace My_steam_server.Services
{
    public class ReportService: IReportsService
    {
        private readonly IReportsRepository _repository;

        public ReportService(IReportsRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ReportMessageModel>> GetReportsAsync() =>
            _repository.GetAllAsync();

        public Task<IEnumerable<ReportMessageModel>> GetReportsByUserAsync(string userId) =>
            _repository.GetByUserIdAsync(userId);

        public async Task AddReportAsync(ReportMessageModel report)
        {
            await _repository.AddAsync(report);
        }
    }
}
