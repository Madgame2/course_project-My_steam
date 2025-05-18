using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Game_Net_DTOLib;
using My_steam_server.Interfaces;
using My_steam_server.Models;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[Controller]")]
    public class LibController:ControllerBase
    {
        private readonly IUserLibraryRepository _userLibraryRepository;
        private readonly IGamesRespository gamesRespository;
        private readonly IGoodRepository goodRepository;

        public LibController(IUserLibraryRepository userLibraryRepository, IGamesRespository gamesRespository, IGoodRepository  goodRepository  )
        {
            _userLibraryRepository = userLibraryRepository;
            this.gamesRespository = gamesRespository;  
            this.goodRepository = goodRepository;
        }

        [HttpGet("Synchronize/{userId}")]
        public async Task<IActionResult> SynchronizeLibrarys([FromRoute] string userId)
        {
            var UserLib = await _userLibraryRepository.GetLibraryByUserIdAsync(userId);
            var outList = new List<SynchronizeLibDto>();

            foreach(var libElem in UserLib)
            {

                var gameInfo = libElem.Game != null ? libElem.Game : await goodRepository.GetByIdAsync(libElem.GameId);
                var newDtoItem = new SynchronizeLibDto
                {
                    UserId = userId,
                    GameId = libElem.GameId,
                    Gamename = gameInfo.Name,
                    DownloadSource = gameInfo.DownloadSource,
                    SpaceRequered = await gamesRespository.GetUncompressedSize(libElem.GameId),
                    PurchaseDate = libElem.PurchaseDate
                };

                outList.Add(newDtoItem);
            }


            return Ok(new NetResponse<List<SynchronizeLibDto>> {Success=true ,data= outList });
        }
    }
}
