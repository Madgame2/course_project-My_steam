using Game_Net_DTOLib;
using My_steam_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Game_Net
{
    public class CartService
    {
        private readonly ComunitationMannageer _commManager;

        public CartService(ComunitationMannageer commManager)
        {
            _commManager = commManager;
        }

        public async Task<bool> addToCart(AddToCarDto dto)
        {
            var json  = JsonSerializer.Serialize(dto);


            NetResponse<bool> response;
            try
            {
                response = await _commManager.SendMessageRest<bool>("api/Cart/Add",Protocol.Https,json);
            }
            catch (UndefinedProtocolException)
            {
                response = await _commManager.SendMessageRest<bool>("api/Cart/Add", Protocol.Http, json);
            }

            if (!response.Success && response.resultCode == ResultCode.PurchouseAlredyExist) throw new PurchouseExistExeption("");

            return true;
        }

        

        public async Task<List<CartItemDto>> getCart(long userId)
        {
            NetResponse<List<CartItemDto>> response;
            try
            {
                response = await _commManager.SendMessageRest<List<CartItemDto>>($"api/Cart/{userId}", Protocol.Https);
            }
            catch (UndefinedProtocolException)
            {
                response = await _commManager.SendMessageRest<List<CartItemDto>>($"api/Cart/{userId}", Protocol.Http);
            }

            return response.data;
        }

        public async Task<bool> DeleteCartElem(DeleteFromCartDTO dto)
        {

            var json = JsonSerializer.Serialize<DeleteFromCartDTO>(dto);
            NetResponse<bool> response;
            try
            {
                response = await _commManager.SendMessageRest<bool>($"api/Cart/delete", Protocol.Https, json);
            }
            catch (UndefinedProtocolException)
            {
                response = await _commManager.SendMessageRest<bool>($"api/Cart/delete", Protocol.Http, json);
            }

            if(response.Success) return true;

            return false;
        }

        public async Task<bool> BuyAllCards(string UserId)
        {
            var json = JsonSerializer.Serialize(new PurchaseDto { UserId = UserId, purchouseIds=new() });
            NetResponse<bool> response;
            try
            {
                response = await _commManager.SendMessageRest<bool>($"api/Buying/Buy", Protocol.Https, json);
            }
            catch (UndefinedProtocolException)
            {
                response = await _commManager.SendMessageRest<bool>($"api/Buying/Buy", Protocol.Http, json);
            }

            return response.data;
        }
    }


}
