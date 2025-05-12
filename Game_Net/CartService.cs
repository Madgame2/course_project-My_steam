using Game_Net_DTOLib;
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
    }
}
