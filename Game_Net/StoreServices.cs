using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Game_Net_DTOLib;
using Microsoft.AspNetCore.Http;

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


        public async Task<ProductDto?> GetGamePageAsync(long GameId)
        {
            try
            {
                try
                {
                    var result = await _commMennager.SendMessageRest<ProductDto>($"api/Goods/GamePage/{GameId}", Protocol.Https);

                    if (result.Success)
                    {
                        return result.data;
                    }
                }
                catch (UndefinedProtocolException ex)
                {
                    var result = await _commMennager.SendMessageRest<ProductDto>($"api/Goods/GamePage/{GameId}", Protocol.Http);

                    if (result.Success)
                    {
                        return result.data;
                    }
                }
            }
            catch(HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new NotFoundExaption(ex.Message);
            }

            return null;
        }

        public async Task<(bool,List<ShowCaseElemDto>?)> tryGetProducts(string queryString)
        {
            string url = $"api/Goods/Games/ShowCase?{queryString}";
            NetResponse<List<ShowCaseElemDto>> serverResponse;
            try
            {
                serverResponse = await _commMennager.SendMessageRest<List<ShowCaseElemDto>>(url, Protocol.Https);
            }
            catch (UndefinedProtocolException)
            {
                serverResponse =  await _commMennager.SendMessageRest<List<ShowCaseElemDto>>(url, Protocol.Http);
            }

            if (serverResponse.Success)
            {
                if(serverResponse.resultCode == ResultCode.noMoreProducts) return (false, serverResponse.data);

                return (true, serverResponse.data);
            }

            return (false, default);
        }
    }
}
