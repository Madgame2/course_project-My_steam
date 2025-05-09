using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace My_steam_server.Controllers
{
    [Route("images/")]
    [ApiController]
    public class ImagesController:ControllerBase
    {
        [HttpGet("{*fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var imageDictinary = Path.Combine(Directory.GetCurrentDirectory(), "resoures/images");
            var imagePath = Path.Combine(imageDictinary, fileName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var fileExtantion = Path.GetExtension(imagePath).ToLowerInvariant();
            string ContentType = fileExtantion switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            var stream  = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            return File(stream, ContentType);
        }
    }
}
