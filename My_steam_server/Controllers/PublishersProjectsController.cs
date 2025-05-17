using Game_Net_DTOLib;
using Microsoft.AspNetCore.Mvc;
using My_steam_server.Interfaces;
using My_steam_server.Models;
using My_steam_server.Services;

namespace My_steam_server.Controllers
{
    [ApiController]
    [Route("api/Projects")]
    public class PublishersProjectsController:ControllerBase
    {

        private readonly string repository = "ProcessedFiles";
        private readonly IGoodRepository<Game> _goodRepository;
        private readonly IPublisherService _publisherService;
        public PublishersProjectsController(IGoodRepository<Game> goodRepository, IPublisherService publisherService)
        {
            _goodRepository = goodRepository;
            _publisherService= publisherService;
        }

        [HttpPost("upload/metadata")]
        public async Task<IActionResult> UploadMetadata(
                [FromForm] string UserId,
                [FromForm] Guid uploadId,
                [FromForm] string projectName,
                [FromForm] string description,
                [FromForm] float price,
                [FromForm] IFormFile headerImage,
                [FromForm] IFormFile libHeader,
                [FromForm] IFormFile libIcon,
                [FromForm] List<IFormFile> screenshots)
        {
            if (!_publisherService.Processed_goods.ContainsKey(uploadId))
            {
                var newGoodModel = await _goodRepository.CreateEmptyModel();
                _publisherService.Processed_goods[uploadId] = newGoodModel;
            }

            var currentGood = _publisherService.Processed_goods[uploadId];

            currentGood.UserId = UserId;
            currentGood.Name = projectName;
            currentGood.Description = description;
            currentGood.Price = price;
            currentGood.ReleaseDate = DateTime.Now;

            var LinksList = await _publisherService.DeployScreenShotsFiles(screenshots, currentGood.Name);
            var outList = new List<Screenshot>();
            foreach (var link in LinksList)
            {
                var newItem = new Screenshot
                {
                    Game = currentGood,
                    Path = link
                };
                outList.Add(newItem);
            }
            currentGood.imageSource = outList;

            currentGood.HeaderImageSource = await _publisherService.DeployHeaderImageAsync(headerImage, currentGood.Name);

            var newPurchaseOption = new My_steam_server.Models.PurchaseOption
            {
                Game = currentGood,
                GameId = currentGood.Id,  // желательно, если Id уже есть
                Price = currentGood.Price,
                PurchaseName = $"Buy {currentGood.Name}",
                ImageLink = currentGood.HeaderImageSource
            };

            newPurchaseOption.GoodsReceived.Add(new GoodReceived
            {
                GoodId = currentGood.Id,
                PurchaseOption = newPurchaseOption,
                // PurchaseOptionId будет установлен автоматически после сохранения
            });

            currentGood.PurchaseOptions.Add(newPurchaseOption);

            await _publisherService.DeployLibImages(libIcon, libHeader, currentGood.Id);


            return Ok(new NetResponse<bool> { Success= true});
        }


        [HttpPost("upload/chunk")]
        public async Task<IActionResult> UploadChunk(
                [FromForm] Guid uploadId,
                [FromForm] int chunkNumber,
                [FromForm] long totalSize,
                [FromForm] IFormFile chunk)
        {
            var CurrentObj = _publisherService.Processed_goods[uploadId];

            var dir = Path.Combine(repository, uploadId.ToString());
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, $"chunk_{chunkNumber}");
            using var fs = new FileStream(path, FileMode.Create);
            await chunk.CopyToAsync(fs);
            return Ok(new NetResponse<bool> { Success=true});
        }

        [HttpPost("upload/finish")]
        public async Task<IActionResult> FinishUpload(
            [FromForm] Guid uploadId)
        {
            var currentObj= _publisherService.Processed_goods[uploadId];

            var dir = Path.Combine(repository, uploadId.ToString());
            var chunkFiles = Directory.GetFiles(dir).OrderBy(f => f);

            using var combinedStream = new ChunkedReadStream(chunkFiles);

            currentObj.DownloadSource = await _publisherService.DeployGameFiles(combinedStream, currentObj.Id);

            Directory.Delete(dir, true);

            await _goodRepository.addAsync(currentObj);

            return Ok(new NetResponse<bool> {Success=true});
        }


        //    [HttpPost("upload")]
        //public async Task<IActionResult> UploadProject()
        //{
        //    var form = await Request.ReadFormAsync();


        //    string? Projectname = form["ProjectName"];
        //    string? description = form["Description"];
        //    string? priceStr = form["Price"];

        //    var headerImage = form.Files.GetFile("HeaderImage");
        //    var zipFile = form.Files.GetFile("ZIPFile");
        //    var libHeader = form.Files.GetFile("LibHeader");
        //    var libIcon = form.Files.GetFile("LibIcon");
        //    var screenshots = form.Files.Where(f => f.Name == "Screenshots").ToList();


        //    if (string.IsNullOrEmpty(Projectname) ||
        //        string.IsNullOrEmpty(description) ||
        //        string.IsNullOrEmpty(priceStr)) return BadRequest();

        //    var newGoodModel = await _goodRepository.CreateEmptyModel();

        //    newGoodModel.Name = Projectname;
        //    newGoodModel.Description = description;
        //    newGoodModel.Price = float.Parse(priceStr);
        //    newGoodModel.ReleaseDate = DateTime.Now;

        //    var LinksList = await _publisherService.DeployScreenShotsFiles(screenshots, newGoodModel.Name);
        //    var outList = new List<Screenshot>();
        //    foreach (var link in LinksList)
        //    {
        //        var newItem = new Screenshot
        //        {
        //            Game = newGoodModel,
        //            Path = link
        //        };
        //        outList.Add(newItem);
        //    }
        //    newGoodModel.imageSource = outList;


        //    //newGoodModel.DownloadSource = await _publisherService.DeployGameFiles(zipFile, newGoodModel.Id);

        //    newGoodModel.HeaderImageSource = await _publisherService.DeployHeaderImageAsync(headerImage, newGoodModel.Name);

        //    var newPurchaseOption = new My_steam_server.Models.PurchaseOption
        //    {
        //        Game = newGoodModel,
        //        GameId = newGoodModel.Id,  // желательно, если Id уже есть
        //        Price = newGoodModel.Price,
        //        PurchaseName = $"Buy {newGoodModel.Name}",
        //        ImageLink = newGoodModel.HeaderImageSource
        //    };

        //    newPurchaseOption.GoodsReceived.Add(new GoodReceived
        //    {
        //        GoodId = newGoodModel.Id,
        //        PurchaseOption = newPurchaseOption,
        //        // PurchaseOptionId будет установлен автоматически после сохранения
        //    });

        //    newGoodModel.PurchaseOptions.Add(newPurchaseOption);

        //    await _publisherService.DeployLibImages(libIcon, libHeader, newGoodModel.Id);

        //    return Ok( new NetResponse<bool> { Success = true, data=true });
        //}
    }
}
