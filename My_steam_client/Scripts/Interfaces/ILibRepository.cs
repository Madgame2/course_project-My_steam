using My_steam_client.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Scripts.Interfaces
{
    public interface ILibRepository
    {
        Task<ManifestRecord?> getRecordByIdAsync(long id);
        Task<bool?> AddRecordAsync(ManifestRecord record);
        Task<ManifestRecord[]> getRecordsByUserIdAsync(long userId);

        Task<bool> UpdateRecordAsync(long id, ManifestRecord record);
    }
}
