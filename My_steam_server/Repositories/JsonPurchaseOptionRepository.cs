using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Text.Json;

namespace My_steam_server.Repositories
{
    public class JsonPurchaseOptionRepository : IPurchaseOptionRepository
    {
        private readonly string _filePath = "purchase_options.json";
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

        public async Task<List<PurchaseOption>> GetAllAsync()
        {
            if (!File.Exists(_filePath))
                return new List<PurchaseOption>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<PurchaseOption>>(json, _options) ?? new List<PurchaseOption>();
        }

        public async Task<PurchaseOption?> GetByIdAsync(long id)
        {
            var all = await GetAllAsync();
            return all.FirstOrDefault(p => p.OptionId == id);
        }

        public async Task AddAsync(PurchaseOption option)
        {
            var all = await GetAllAsync();
            all.Add(option);
            await SaveAllAsync(all);
        }

        public async Task UpdateAsync(PurchaseOption option)
        {
            var all = await GetAllAsync();
            var index = all.FindIndex(p => p.OptionId == option.OptionId);
            if (index != -1)
            {
                all[index] = option;
                await SaveAllAsync(all);
            }
        }

        public async Task DeleteAsync(long id)
        {
            var all = await GetAllAsync();
            var toRemove = all.FirstOrDefault(p => p.OptionId == id);
            if (toRemove != null)
            {
                all.Remove(toRemove);
                await SaveAllAsync(all);
            }
        }

        private async Task SaveAllAsync(List<PurchaseOption> all)
        {
            var json = JsonSerializer.Serialize(all, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
