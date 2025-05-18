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
        private readonly IGoodsService _goodsService_games;

        public GoodsController(IGoodsService goodsService_games)
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

        [HttpGet("GamePage/{id:int}")]
        public async Task<IActionResult> GetGamepage(int id)
        {
            var result = await _goodsService_games.GetGoodByIdAsync(id);

            if (result.resultCode == ResultCode.WrongGoodId)
                return NotFound();

            var game = result.data;

            // Преобразование данных в DTO
            var gameDto = new Game_Net_DTOLib.ProductDto
            {
                GameId = game.Id,
                GameName = game.Name,
                Description = game.Description,
                ImageLink = game.HeaderImageSource,
                ReleaseDate = game.ReleaseDate,
                rating = game.ratinng, // Убедись, что свойство в DTO называется правильно
                imagesLinks = game.imageSource.Select(p=>p.Path).ToList(), // Преобразование массива в список
                PurchaseOptions = game.PurchaseOptions.Select(po => new Game_Net_DTOLib.PurchaseOption
                {
                    PurchaseId = po.OptionId, // Преобразуем из модели в DTO
                    GameName = po.PurchaseName,
                    Price = po.Price // Оставляем как float, без изменения в строку
                }).ToList() // Преобразуем все варианты покупки
            };

            var response = new NetResponse<ProductDto> { Success = true, data = gameDto };

            return Ok(response);
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

        [HttpGet("Games/ShowCase")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilterDto filter)
        {
            var Showcase = await _goodsService_games.GetProductsAsync(filter);
            var responseList = new List<ShowCaseElemDto>();
            foreach (var item in Showcase.data)
            {
                var new_Item= new ShowCaseElemDto();
                new_Item.description = item.Description;
                new_Item.price = item.Price;
                new_Item.title = item.Name;
                new_Item.productId = item.Id;
                new_Item.headerImageSource = item.HeaderImageSource;

                responseList.Add(new_Item);
            }

            return Ok(new NetResponse<List<ShowCaseElemDto>> { Success = Showcase.Success,resultCode=Showcase.resultCode,data=responseList });
        }
    }
}
