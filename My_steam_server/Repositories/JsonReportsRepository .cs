using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace My_steam_server.Repositories
{
    public class JsonReportsRepository : IReportsRepository
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _lock = new(1, 1);


        public JsonReportsRepository(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }
        public async Task<IEnumerable<ReportMessageModel>> GetAllAsync()
        {
            await _lock.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                    return new List<ReportMessageModel>();

                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<ReportMessageModel>>(json) ?? new List<ReportMessageModel>();
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<IEnumerable<ReportMessageModel>> GetByUserIdAsync(string userId)
        {
            var allReports = await GetAllAsync();
            return allReports.Where(r => r.UserID == userId);
        }

        public async Task AddAsync(ReportMessageModel report)
        {
            await _lock.WaitAsync();
            try
            {
                var reports = (await GetAllAsync()).ToList();
                reports.Add(report);

                var json = JsonSerializer.Serialize(reports, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                });

                await File.WriteAllTextAsync(_filePath, json);
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
