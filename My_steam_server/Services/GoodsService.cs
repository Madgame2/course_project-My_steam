using Game_Net_DTOLib;
using Microsoft.AspNetCore.Http.HttpResults;
using My_steam_server.Interfaces;
using My_steam_server.Models;

namespace My_steam_server.Services
{
    public class GoodsService : IGoodsService
    {
        private readonly IGoodRepository _goodRepository;

        public GoodsService(IGoodRepository goodRepository) { 
            _goodRepository = goodRepository;
        }

        public async Task<NetResponse<bool>> addGoodAsync(Game good)
        {
            var existing = await _goodRepository.HasObject(good);

            if(existing) return new NetResponse<bool> { Success =false, resultCode=ResultCode.ObjectAleradyExist, Message="This Good already exist in repository", data=existing};

            bool result = await _goodRepository.addAsync(good);

            if(result) return new NetResponse<bool> { Success = true, data=result };

            return new NetResponse<bool> { Success= false, resultCode=ResultCode.UnKnowError};
        }

        public async Task<NetResponse<List<Game>>> GetAll()
        {
            var result = await _goodRepository.GetAll();

            return new NetResponse<List<Game>> { Success = true, data = result };
        }

        public async Task<NetResponse<Game?>> GetGoodByIdAsync(int goodId)
        {
            var good = await _goodRepository.GetByIdAsync(goodId);

            if (good == null) {
                return new NetResponse<Game?>
                {
                    Success = false,
                    resultCode = ResultCode.WrongGoodId,
                    data = default
                };
            }

            return new NetResponse<Game?>
            {
                Success = true,
                data = good
            };
        }

        public async Task<NetResponse<List<Game>>> GetProductsAsync(ProductFilterDto filters)
        {
            var ProductsList = await _goodRepository.GetPagesAsync(filters);

            if (ProductsList.Count <= filters.PageSize) return new NetResponse<List<Game>> { Success = true, resultCode = ResultCode.noMoreProducts, data = ProductsList };

            return new NetResponse<List<Game>> { Success=true, data = ProductsList };
        }
    }
}
