using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Text.Json;

namespace My_steam_server.Repositories
{
    public class JsonGoodsRepository<T> : IGoodRepository<T> where T: Good
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

        public async Task<List<T>> GetAll()
        {
            string json = await File.ReadAllTextAsync(_filePath);

            var result = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

            return result;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            string json = await File.ReadAllTextAsync(_filePath);

            var objects = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>(); 

            return objects.FirstOrDefault(e => e.Id == id);
        }

        public async Task<bool> addAsync(T entity)
        {
            string json = await File.ReadAllTextAsync(_filePath);

            var objects = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

            int id = getFreeId(objects);

            entity.Id = id;

            objects.Add(entity);
            objects.Sort((a,b)=>a.Id.CompareTo(b.Id));

            await saveAsync(objects);


            return true;
        }

        private async Task saveAsync(List<T> data)
        {
            await using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, data);
        }


        private int getFreeId(List<T> objects)
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
    }
}
