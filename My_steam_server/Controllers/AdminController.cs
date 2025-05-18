using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_steam_server.Interfaces;
using Game_Net_DTOLib;
using Microsoft.EntityFrameworkCore;
using My_steam_server.Models;

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

        [HttpPost("Synchronize")]
        public async Task<IActionResult> Synchronize([FromBody] SynchronizeDto dto)
        {
            if (dto.Users != null)
            {
                foreach (var user in dto.Users)
                {
                    var existingUser = await _userRepository.GetByIdAsync(user.UserId);
                    if (user.IsNew || existingUser == null)
                    {

                    }
                    else
                    {
                        existingUser.Email = user.Email;
                        existingUser.UserName = user.UserName;
                        existingUser.Role = user.Role;
                        existingUser.RigisterDate = user.registerDate;

                        await _userRepository.SaveChangesAsync();
                    }
                }
            }

            if (dto.Games != null)
            {
                foreach (var game in dto.Games)
                {
                    var existingGame = await _goodRepository.GetByIdAsync(game.GameId);
                    if (game.IsNew || existingGame == null)
                    {
                    }
                    else
                    {
                        existingGame.Name = game.GameName;
                        existingGame.Description = game.Descritption;
                        existingGame.HeaderImageSource = game.HeaderImageSource;
                        existingGame.UserId = game.UserId;
                        existingGame.DownloadSource = game.DownloadedSource;
                        existingGame.ratinng = game.Rating;
                        existingGame.ReleaseDate = game.ReliseDate;
                        existingGame.Price = game.Price;

                        await _goodRepository.UpdateAsync(existingGame);
                    }
                }
            }

            if (dto.Purhcases != null)
            {
                foreach (var purchase in dto.Purhcases)
                {

                    var existing = await _purchaseOptionRepository.GetByIdAsync(purchase.PurhcaseId);
                    if (purchase.IsNew || existing == null)
                    {

                    }
                    else
                    {
                        existing.Price = purchase.Price;
                        existing.PurchaseName = purchase.PurhcaseName;
                        existing.ImageLink = purchase.imageLinnk;
                        existing.GameId = purchase.GameID;
                        await _purchaseOptionRepository.UpdateAsync(existing);
                    }
                }
            }

            return Ok(new NetResponse<bool> { Success = true });
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
