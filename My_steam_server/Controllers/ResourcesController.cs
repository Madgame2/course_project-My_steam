using Microsoft.AspNetCore.Mvc;
using My_steam_server.Interfaces;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourcesController : ControllerBase
    {

        public readonly IResources resources;

        public ResourcesController(IResources resources)
        {
            this.resources = resources;
        }

        [HttpGet("Markdown/{fileName}")]
        public async Task<IActionResult> GetMarkdownStream(string fileName)
        {
            var stream = await resources.GetMarkdownStreamAsync(fileName);

            if (stream == null)
            {
                return NotFound();
            }

            return File(stream, "text/plain", fileName);
        }
    }
}
