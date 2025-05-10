using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using My_steam_server.Interfaces;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class DownLoadController:ControllerBase
    {
        private readonly IGamesRespository _gamesRepository;

        public DownLoadController(IGamesRespository gamesRespository)
        {
            _gamesRepository = gamesRespository;
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> DownloadGame(int gameId)
        {
            if (!(await _gamesRepository.GameExistsAsync(gameId)))
                return NotFound();

            var filePath = await _gamesRepository.GetGameFilePathAsync(gameId);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");

            var fileInfo = new FileInfo(filePath);
            var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                FileOptions.Asynchronous | FileOptions.SequentialScan
            );

            // Устанавливаем заголовки для поддержки докачки
            Response.Headers["Accept-Ranges"] = "bytes";
            Response.Headers["Content-Disposition"] = $"attachment; filename=\"game_{gameId}.zip\"";

            return new FileStreamResult(stream, "application/octet-stream")
            {
                EnableRangeProcessing = true,
                LastModified = fileInfo.LastWriteTimeUtc,
                EntityTag = new EntityTagHeaderValue($"\"{fileInfo.LastWriteTimeUtc.Ticks:x}\"")
            };
        }
    }
}
