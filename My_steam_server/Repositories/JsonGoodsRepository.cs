using Game_Net_DTOLib;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Text.Json;

namespace My_steam_server.Repositories
{
    public class JsonGoodsRepository : IGoodRepository
    {
        public static string _filePath=string.Empty;

        public JsonGoodsRepository(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");

            }
            
        }

        private async Task<List<Game>> GetObjets()
        {
            string json = await File.ReadAllTextAsync(_filePath);

            var result = JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();

            return result;
        }

        public async Task<List<Game>> GetAll()
        {
            var result = await GetObjets();

            return result;
        }

        public async Task<Game?> GetByIdAsync(long id)
        {
            var objects = await GetObjets();

            return objects.FirstOrDefault(e => e.Id == id);
        }

        public async Task<bool> addAsync(Game entity)
        {
            var objects = await GetObjets();

            //int id = getFreeId(objects);

            //entity.Id = id;

            objects.Add(entity);
            objects.Sort((a,b)=>a.Id.CompareTo(b.Id));

            //await saveAsync(objects);


            return true;
        }

        private async Task saveAsync(List<Game> data)
        {
            await using var stream = File.Create(_filePath);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            await JsonSerializer.SerializeAsync(stream, data, options);
        }


        private int getFreeId(List<Game> objects)
        {
            int curentId = 0;

            if(objects.Count==0) return curentId;

            foreach (var obj in objects)
            {
                if(obj.Id != curentId) return curentId;

                curentId++;
            }

            return curentId;
        }

        private async Task<int> getFreeId()
        {

            var objects = await GetObjets();
            int curentId = 1;

            if (objects.Count == 1) return curentId;

            foreach (var obj in objects)
            {
                if (obj.Id != curentId) return curentId;

                curentId++;
            }

            return curentId;
        }

        public async Task<bool> HasObject(Game entity)
        {

            var objects = await GetObjets();

            return objects.Any(obj => obj.Equals(entity)) ? true : false;
        }

        public async Task<List<Game>> GetPagesAsync(ProductFilterDto filter)
        {
            var all = await GetObjets(); // предположим, что возвращает List<T>

            var query = all
                .OrderBy(x => x.Id)
                .AsEnumerable();

            
            if (filter.LastSeenId.HasValue)
                query = query.Where(x => x.Id > filter.LastSeenId.Value);

            
            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x =>
                    (x as dynamic).Name.ToString().Contains(filter.Search, StringComparison.OrdinalIgnoreCase));


            if (filter.minPrice.HasValue)
                query = query.Where(x => x .Price >= filter.minPrice.Value);


            if (filter.maxPrice.HasValue)
                query = query.Where(x => x .Price <= filter.maxPrice.Value);


            return query
                .Take(filter.PageSize)
                .ToList();
        }

        public async Task<Game> CreateEmptyModel(string id)
        {
            return new Game { Id = await getFreeId() };
        }

        public Task<bool> UpdateAsync(Game entity)
        {
            throw new NotImplementedException();
        }

    }
}
