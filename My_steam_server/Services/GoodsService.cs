using Game_Net_DTOLib;
using Microsoft.AspNetCore.Http.HttpResults;
using My_steam_server.Interfaces;
using My_steam_server.Models;

namespace My_steam_server.Services
{
    public class GoodsService<T> : IGoodsService<T> where T : Good
    {
        private readonly IGoodRepository<T> _goodRepository;

        public GoodsService(IGoodRepository<T> goodRepository) { 
            _goodRepository = goodRepository;
        }

        public async Task<NetResponse<bool>> addGoodAsync(T good)
        {
            var existing = await _goodRepository.HasObject(good);

            if(existing) return new NetResponse<bool> { Success =false, resultCode=ResultCode.ObjectAleradyExist, Message="This Good already exist in repository", data=existing};

            bool result = await _goodRepository.addAsync(good);

            if(result) return new NetResponse<bool> { Success = true, data=result };

            return new NetResponse<bool> { Success= false, resultCode=ResultCode.UnKnowError};
        }

        public async Task<NetResponse<List<T>>> GetAll()
        {
            var result = await _goodRepository.GetAll();

            return new NetResponse<List<T>> { Success = true, data = result };
        }

        public async Task<NetResponse<T?>> GetGoodByIdAsync(int goodId)
        {
            var good = await _goodRepository.GetByIdAsync(goodId);

            if (good == null) {
                return new NetResponse<T?>
                {
                    Success = false,
                    resultCode = ResultCode.WrongGoodId,
                    data = default
                };
            }

            return new NetResponse<T?>
            {
                Success = true,
                data = good
            };
        }

        public async Task<NetResponse<List<T>>> GetProductsAsync(ProductFilterDto filters)
        {
            var ProductsList = await _goodRepository.GetPagesAsync(filters);

            if (ProductsList.Count <= filters.PageSize) return new NetResponse<List<T>> { Success = true, resultCode = ResultCode.noMoreProducts, data = ProductsList };

            return new NetResponse<List<T>> { Success=true, data = ProductsList };
        }
    }
}
