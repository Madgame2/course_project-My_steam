using Game_Net_DTOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Game_Net
{
    public class AdminService
    {
        private readonly Game_Net.ComunitationMannageer Mannager;

        public AdminService(ComunitationMannageer mannager)
        {
            Mannager = mannager;
        }

        public async Task<List<UserDB_dto>> GetUsers()
        {
            try
            {
                var result = await Mannager.SendMessageRest<List<UserDB_dto>>("api/Admin/get/User", Protocol.Http);

                return result.data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GamesDB_dto>> GetGames()
        {
            try
            {
                var result = await Mannager.SendMessageRest<List<GamesDB_dto>>("api/Admin/get/Games", Protocol.Http);

                return result.data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<PurhcaseOptionsDB_dto>> GetPurhcaseOptions()
        {
            try
            {
                var result = await Mannager.SendMessageRest<List<PurhcaseOptionsDB_dto>>("api/Admin/get/PurchaseOptions", Protocol.Http);

                return result.data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<DetachetLibDB_dto>> GetDetachedLib()
        {
            try
            {
                var result = await Mannager.SendMessageRest<List<DetachetLibDB_dto>>("api/Admin/get/Lib", Protocol.Http);

                return result.data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<ResivedGoodsDB_dto>> GetResivedGoods()
        {
            try
            {
                var result = await Mannager.SendMessageRest<List<ResivedGoodsDB_dto>>("api/Admin/get/ResivedGoods", Protocol.Http);

                return result.data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Synchronize( SynchronizeDto dto)
        {

            var json = JsonSerializer.Serialize(dto);
            try
            {
                var result = await Mannager.SendMessageRest<List<ResivedGoodsDB_dto>>("api/Admin/Synchronize", Protocol.Http, json);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
