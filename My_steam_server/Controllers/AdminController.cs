using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_steam_server.Interfaces;
using Game_Net_DTOLib;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Admin")]
    public class AdminController:ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IGoodRepository _goodRepository;
        private readonly IPurchaseOptionRepository _purchaseOptionRepository;
        private readonly IUserLibraryRepository _userLibraryRepository;

        public AdminController(IUserRepository userRepository, IGoodRepository goodRepository, IPurchaseOptionRepository purchaseOptionRepository, IUserLibraryRepository userLibraryRepository)
        {
            _userRepository = userRepository;
            _goodRepository = goodRepository;
            _purchaseOptionRepository = purchaseOptionRepository;
            _userLibraryRepository = userLibraryRepository;
        }


        [HttpGet("get/User")]
        public async Task<IActionResult> GetUserDB()
        {
            var allData = await _userRepository.GetAllAsync();

            var outList = new List<Game_Net_DTOLib.UserDB_dto>();
            foreach (var item in allData)
            {
                var newDTO = new Game_Net_DTOLib.UserDB_dto {
                
                    Email= item.Email,
                    registerDate=item.RigisterDate,
                    UserName = item.UserName,
                    Role = item.Role,
                    UserId = item.Id,
                };

                outList.Add(newDTO);
            }

            return Ok(new NetResponse<List<UserDB_dto>> { Success = true, data = outList });
        }


        [HttpGet("get/Games")]
        public async Task<IActionResult> GetGamesDB()
        {
            var allData = await _goodRepository.GetAll();

            var outList = new List<Game_Net_DTOLib.GamesDB_dto>();
            foreach (var item in allData)
            {
                var newDTO = new Game_Net_DTOLib.GamesDB_dto
                {
                    Descritption=item.Description,
                    DownloadedSource=item.DownloadSource,
                    GameId=item.Id,
                    GameName=item.Name,
                    HeaderImageSource=item.HeaderImageSource,
                    Price=item.Price,
                    Rating=item.ratinng,
                    ReliseDate=item.ReleaseDate,
                    UserId=item.UserId
                };

                outList.Add(newDTO);
            }

            return Ok(new NetResponse<List<GamesDB_dto>> { Success = true, data = outList });
        }

        [HttpGet("get/PurchaseOptions")]
        public async Task<IActionResult> GetPurchseOptionsDB()
        {
            var allData = await _purchaseOptionRepository.GetAllAsync();

            var outList = new List<Game_Net_DTOLib.PurhcaseOptionsDB_dto>();
            foreach (var item in allData)
            {
                var newDTO = new Game_Net_DTOLib.PurhcaseOptionsDB_dto
                {
                    GameID=item.GameId,
                    imageLinnk=item.ImageLink,
                    Price = item.Price,
                    PurhcaseId=item.OptionId,
                    PurhcaseName=item.PurchaseName
                };

                outList.Add(newDTO);
            }

            return Ok(new NetResponse<List<PurhcaseOptionsDB_dto>> { Success = true, data = outList });
        }

        [HttpGet("get/Lib")]
        public async Task<IActionResult> GetLibDB()
        {
            var allData = await _userLibraryRepository.GetAllAsync();

            var outList = new List<Game_Net_DTOLib.DetachetLibDB_dto>();
            foreach (var item in allData)
            {
                var newDTO = new Game_Net_DTOLib.DetachetLibDB_dto
                {
                    GameId=item.GameId,
                    PurchaseDate=item.PurchaseDate,
                    UserId=item.UserId,
                };

                outList.Add(newDTO);
            }

            return Ok(new NetResponse<List<DetachetLibDB_dto>> { Success = true, data = outList });
        }

        //[HttpGet("get/ResivedGoods")]
        //public async Task<IActionResult> GeResivedGoodsDB()
        //{
        //    var allData = await _purchaseOptionRepository.GetAllAsync();

        //    allData[0].GoodsReceived

        //    var outList = new List<Game_Net_DTOLib.DetachetLibDB_dto>();
        //    foreach (var item in allData)
        //    {
        //        var newDTO = new Game_Net_DTOLib.DetachetLibDB_dto
        //        {
        //            GameId = item.GameId,
        //            PurchaseDate = item.PurchaseDate,
        //            UserId = item.UserId,
        //        };

        //        outList.Add(newDTO);
        //    }

        //    return Ok(new NetResponse<List<DetachetLibDB_dto>> { Success = true, data = outList });
        //}
    }
}
