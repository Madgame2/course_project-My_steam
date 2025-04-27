using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Net_DTOLib;

namespace Game_Net
{
    public class StoreServices
    {

        private readonly ComunitationMannageer _commMennager;

        public StoreServices(ComunitationMannageer commMennager)
        {
            _commMennager = commMennager;
        }

        public async Task<List<GameSliderDto>> GetSliderObjects(int count)
        {
            try
            {
                var result = await _commMennager.SendMessageRest<List<GameSliderDto>>($"api/Goods/Games/recomended/{count}", Protocol.Https);

                if (result.Success)
                {
                    return result.data;
                }


            }catch(UndefinedProtocolException ex)
            {
                Console.Write(ex.Message);

                try
                {
                    var result = await _commMennager.SendMessageRest<List<GameSliderDto>>($"api/Goods/Games/recomended/{count}", Protocol.Http);

                    return result.data;
                }
                catch (UndefinedProtocolException)
                {
                    throw new Exception("cant send message");
                }
            }

            return null;
        }
    }
}
