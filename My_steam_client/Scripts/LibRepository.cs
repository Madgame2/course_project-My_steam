using My_steam_client.Scripts.Interfaces;
using My_steam_client.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace My_steam_client.Scripts
{
    public class LibRepository:ILibRepository
    {
        public static string ManifestFilePath {  get; set; }

        public async Task<ManifestRecord?> getRecordByIdAsync(long id)
        {
            var json = await File.ReadAllTextAsync(ManifestFilePath);

            var objects = JsonSerializer.Deserialize<List<ManifestRecord>>(json);

            return objects?.FirstOrDefault(p=>p.RecordId==id);
        }

        public async Task<bool?> AddRecordAsync(ManifestRecord record)
        {
            var objects = await GetAllRecordsAsync();

            record.RecordId = getFreeId(objects);

            objects.Add(record);


            return true;
        }

        public async Task<ManifestRecord[]> getRecordsByUserIdAsync(long userId)
        {
            var objects = await GetAllRecordsAsync();

            return objects.Where(p=>p.UserId==userId).ToArray();
        }


        private async  Task<List<ManifestRecord>> GetAllRecordsAsync()
        {
            var json = await File.ReadAllTextAsync(ManifestFilePath);

            return JsonSerializer.Deserialize<List<ManifestRecord>>(json)?.ToList() ?? new List<ManifestRecord>();
        }

        private long getFreeId(List<ManifestRecord> records) {
            int Current_id = 1;

            foreach (var record in records) {
                if (record.RecordId == Current_id) Current_id++;
                else break;
            }

            return Current_id;
        }
        private async Task saveChanges(List<ManifestRecord> objects)
        {
            objects.Sort((a, b) => a.RecordId.CompareTo(b.RecordId));

            string json = JsonSerializer.Serialize(objects);
            await File.WriteAllTextAsync(ManifestFilePath, json);
        }

        public async Task<bool> UpdateRecordAsync(long id, ManifestRecord record)
        {
            var objectsList = await GetAllRecordsAsync();
            var searchedRecord = objectsList.FirstOrDefault(p=>p.RecordId==id);


            if (searchedRecord == null) return false;

            searchedRecord = record;

            await saveChanges(objectsList);

            return true;
        }
    }
}
