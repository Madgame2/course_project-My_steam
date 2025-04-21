using My_steam_client.Scripts.Interfaces;
using Game_Net_DTOLib;
using Game_Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Scripts.Services
{
    public class client_PingService : IPingService_client
    {
        private readonly Game_Net.PingService pingService;
        private bool lastPingResult = false;

        public bool lastPing => lastPingResult;

        public client_PingService(Game_Net.ComunitationMannageer mannageer)
        {
            pingService = new Game_Net.PingService(mannageer);
        }

        public async Task<bool> PingAsync(int maxAttemps=5, int timeOutMS=5000, int retryDelayM=1000)
        {
            var result = await pingService.PingAync(maxAttemps, timeOutMS, retryDelayM);

            lastPingResult = result.status == ServerStatus.Online;

            return lastPingResult;
        }
    }
}
