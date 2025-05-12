using Game_Net_DTOLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_steam_server.Interfaces;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    public class BuyingController:ControllerBase
    {
        private readonly IBoughtService boughtService;

        public BuyingController(IBoughtService boughtService)
        {
            this.boughtService = boughtService;
        }

        [HttpPost("Buy")]
        public async Task<IActionResult> BuyAll([FromBody] PurchaseDto dto)
        {
            var result = await boughtService.buyUserCart(dto.UserId);

            return Ok(new NetResponse<bool> { Success=true,data=result});
        }
    }
}
