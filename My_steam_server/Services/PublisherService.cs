using My_steam_server.Interfaces;
using My_steam_server;
using System.IO;
using System.IO.Compression;
using My_steam_server.Repositories;
using My_steam_server.Models;


namespace My_steam_server.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IGamesRespository _gameRepository;
        private readonly IScreenShotsRepository _screenShotsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGamesStaticFilesRepository _gamesStaticFilesRepository;

        public Dictionary<Guid, Game> Processed_goods { get; set; } = new();

        public PublisherService(IGamesRespository gameRepository, IScreenShotsRepository screenShotsRepository, IHttpContextAccessor httpContextAccessor, IGamesStaticFilesRepository gamesStaticFilesRepository)
        {
            _gameRepository = gameRepository;
            _screenShotsRepository = screenShotsRepository;
            _httpContextAccessor = httpContextAccessor;
            _gamesStaticFilesRepository = gamesStaticFilesRepository;
        }

        public async Task<string> DeployGameFiles(Stream? stream, long GameId)
        {
            if(stream == null) throw new ArgumentNullException(nameof(stream));

            try
            {
                string repositoryPath = _gameRepository.getRepositoryPath();
                string filePath = Path.Combine(repositoryPath, $"game_{GameId}.zip");

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!); // на всякий случай

                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await stream.CopyToAsync(fileStream);

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new InvalidOperationException("HttpContext is not available");


                string scheme = httpContext.Request.Scheme;
                string host = httpContext.Request.Host.Value;
                string baseUrl = $"{scheme}://{host}";

                return $"{baseUrl}/Download/{GameId}";
            }
            catch (IOException ex)
            {
                throw new ApplicationException("Ошибка при записи файла на диск.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Произошла неизвестная ошибка при развертывании игры.", ex);
            }
        }

        public async Task<string> DeployHeaderImageAsync(IFormFile File, string GameName)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidOperationException("HttpContext is not available");


            string scheme = httpContext.Request.Scheme;
            string host = httpContext.Request.Host.Value;
            string baseUrl = $"{scheme}://{host}";

            var safeGameName = CommonMethods.SanitizeFolderName(GameName);
            var RelativePaths = await _screenShotsRepository.saveFileAsync(File, safeGameName);

            var fullUrls = $"{baseUrl}/{RelativePaths.Replace("\\", "/")}";

            return fullUrls;
        }

        public async Task DeployLibImages(IFormFile LibIcon, IFormFile LibHeader, long gameId)
        {
            if (LibIcon == null) throw new ArgumentNullException(nameof(LibIcon));
            if (LibHeader == null) throw new ArgumentNullException(nameof(LibHeader));

            var zipStream = new MemoryStream();

            using (var archive = new System.IO.Compression.ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                // Функция для добавления файла в архив с нужным именем
                async Task AddFileToArchive(IFormFile file, string entryName)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var zipEntry = archive.CreateEntry($"{entryName}{extension}", CompressionLevel.Optimal);

                    using var entryStream = zipEntry.Open();
                    await file.CopyToAsync(entryStream);
                }

                await AddFileToArchive(LibIcon, "icon");
                await AddFileToArchive(LibHeader, "header");
            }

            zipStream.Position = 0;

            await _gamesStaticFilesRepository.SaveNewFile(zipStream, Convert.ToInt32(gameId));
        }

        public async Task<List<string>> DeployScreenShotsFiles(List<IFormFile> Files, string GameName)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidOperationException("HttpContext is not available");


            string scheme = httpContext.Request.Scheme;
            string host = httpContext.Request.Host.Value;
            string baseUrl = $"{scheme}://{host}";

            var safeGameName = CommonMethods.SanitizeFolderName(GameName);
            var RelativePaths = await _screenShotsRepository.saveFilesAsync(Files, safeGameName);

            var fullUrls = RelativePaths.Select(relPath => $"{baseUrl}/{relPath.Replace("\\", "/")}").ToList();

            return fullUrls;
        }
    }
}
