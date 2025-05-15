using Game_Net_DTOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Game_Net
{
    public class ReportsService
    {
        private readonly ComunitationMannageer _commManager;

        public ReportsService(ComunitationMannageer commManager)
        {
            _commManager = commManager;
        }

        public async Task SendMessage(SendReportDTO dto)
        {
            var json = JsonSerializer.Serialize(dto);


            try
            {
                await _commManager.SendMessageRest("api/reports", Protocol.Https, json);
            }
            catch (UndefinedProtocolException)
            {
                await _commManager.SendMessageRest("api/reports", Protocol.Http, json);

            }
        }
    }
}
