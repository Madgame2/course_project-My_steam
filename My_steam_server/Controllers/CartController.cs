using Game_Net_DTOLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_steam_server.Interfaces;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CartController:ControllerBase
    {

        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCarDto dto)
        {
            var result = await _cartService.AddToCart(dto.useerId, dto.purchouseID);

            if (!result.Success && result.resultCode == ResultCode.UnKnowError) return BadRequest();

            return Ok(result);
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetUseerCart([FromRoute] string UserId)
        {
            var result = await _cartService.getUserCart(UserId);

            if (!result.Success && result.resultCode == ResultCode.UnKnowError) return BadRequest();

            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> deleteCartItem([FromBody] Game_Net_DTOLib.DeleteFromCartDTO dto)
        {
            var result = await _cartService.deleteCartElem(dto.UserId, dto.CartId);

            if (!result.Success && result.resultCode == ResultCode.UnKnowError) return BadRequest();

            return Ok(result);
        }
    }
}
