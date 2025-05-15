using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Game_Net_DTOLib;
using My_steam_server.Repositories;
using My_steam_server.Models;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportService;

        public ReportsController(IReportsService reportService)
        {
            _reportService = reportService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _reportService.GetReportsAsync();
            return Ok(new NetResponse<IEnumerable<ReportMessageModel>> { Success = true, data = reports });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var reports = await _reportService.GetReportsByUserAsync(userId);
            return Ok(new NetResponse<IEnumerable<ReportMessageModel>> { Success = true, data = reports });
        }


        [HttpPost]
        public async Task<IActionResult> AddReport([FromBody] SendReportDTO report)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newReport = new ReportMessageModel
            {
                UserID = report.UserId,
                Message = report.Message,
                ReportDate = DateTime.Now,
                Topic = report.Topic,
            };

            try
            {
                await _reportService.AddReportAsync(newReport);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

    }
}
