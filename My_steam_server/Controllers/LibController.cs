using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Game_Net_DTOLib;
using My_steam_server.Interfaces;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[Controller]")]
    public class LibController:ControllerBase
    {
        private readonly IUserLibraryRepository _userLibraryRepository;
        private readonly IGamesRespository gamesRespository;

        public LibController(IUserLibraryRepository userLibraryRepository, IGamesRespository gamesRespository)
        {
            _userLibraryRepository = userLibraryRepository;
            this.gamesRespository = gamesRespository;   
        }

        [HttpGet("Synchronize/{userId}")]
        public async Task<IActionResult> SynchronizeLibrarys([FromRoute] string userId)
        {
            var UserLib = await _userLibraryRepository.GetLibraryByUserIdAsync(userId);
            var outList = new List<SynchronizeLibDto>();

            foreach(var libElem in UserLib)
            {
                var newDtoItem = new SynchronizeLibDto
                {
                    UserId = userId,
                    GameId = libElem.GameId,
                    Gamename = libElem.Game.Name,
                    DownloadSource =libElem.Game.DownloadSource,
                    SpaceRequered = await gamesRespository.GetUncompressedSize(libElem.GameId)
                };

                outList.Add(newDtoItem);
            }


            return Ok(new NetResponse<List<SynchronizeLibDto>> {Success=true ,data= outList });
        }
    }
}
