using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Game_Net_DTOLib;

namespace Game_Net
{
    public class LibraryService
    {
        private readonly ComunitationMannageer _commManager;

        public LibraryService(ComunitationMannageer commManager)
        {
            _commManager = commManager;
        }


        public async Task<List<SynchronizeLibDto>> GerDetachedLib(string UserID)
        {
            NetResponse<List<SynchronizeLibDto>> result;
            try
            {
                result = await _commManager.SendMessageRest<List<SynchronizeLibDto>>($"Lib/Synchronize/{UserID}", Protocol.Https);
            }
            catch (UndefinedProtocolException)
            {
                result = await _commManager.SendMessageRest<List<SynchronizeLibDto>>($"Lib/Synchronize/{UserID}", Protocol.Http);

            }

            return result.data;
        }
    }
}
