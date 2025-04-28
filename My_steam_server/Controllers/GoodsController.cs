using Game_Net_DTOLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_steam_server.Interfaces;
using My_steam_server.Models;

namespace My_steam_server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GoodsController : ControllerBase
    {
        private readonly IGoodsService<Game> _goodsService_games;

        public GoodsController(IGoodsService<Game> goodsService_games)
        {
            _goodsService_games = goodsService_games;
        }

        [HttpPost("Game")]
        public async Task<IActionResult> AddGame(Game game)
        {

            var result = await _goodsService_games.addGoodAsync(game);

            if (result.Success) return Ok(result);


            return BadRequest(result);
        }

        [HttpGet("Game/{id:int}")]
        public async Task<IActionResult> GetGame(int id)
        {
            var game = await _goodsService_games.GetGoodByIdAsync(id);

            if (game.resultCode == ResultCode.WrongGoodId) return NotFound();

            return Ok(game);
        }

        [HttpGet("Game")]
        public async Task<IActionResult> GetAllGame(int id)
        {
            var result = await _goodsService_games.GetAll();

            return Ok(result);
        }



        [HttpGet("Games/recomended/{count:int}")]
        public async Task<IActionResult> GetRecomendet(int count)
        {
            var allObjcets = (await _goodsService_games.GetAll()).data;
            List<GameSliderDto> result = new List<GameSliderDto>();

            for(int i = 0; i < count && i < allObjcets.Count; i++)
            {
                var current = allObjcets[i];
                GameSliderDto newObj = new GameSliderDto();

                newObj.GameId = current.Id;
                newObj.Name = current.Name;
                newObj.price = current.Price;
                newObj.Description = current.Description;
                newObj.imageLink = current.HeaderImageSource;

                result.Add(newObj);
            }

            return Ok(new NetResponse<List<GameSliderDto>> { Success=true, data=result});
        }
    }
}
