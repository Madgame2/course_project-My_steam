using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net.Interfaces
{
    public interface IRestClient
    {
        Task<string> GetAsync(string endPoint);
        Task<string> PostAsync(string endPoint, string jsonData);
    }
}
