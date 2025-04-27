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

    }
}
