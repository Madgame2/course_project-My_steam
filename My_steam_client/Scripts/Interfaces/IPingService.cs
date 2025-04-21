using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Scripts.Interfaces
{
    public interface IPingService_client
    {
        public Task<bool> PingAsync(int maxAttemps = 5, int timeOutMS = 5000, int retryDelayM = 1000);
    }
}
