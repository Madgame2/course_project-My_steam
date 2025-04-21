using Microsoft.AspNetCore.Mvc;
using Game_Net_DTOLib;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        public PingController() { }

        public IActionResult Ping() {

            var result = new NetResponse<PingDto> { Success = true, data = new PingDto { date = DateTime.Now, status = ServerStatus.Online } };

            return Ok( result);
        }
    }
}
