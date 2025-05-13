using My_steam_server.Interfaces;
using My_steam_server.Repositories;
using System.ComponentModel.Design;

namespace My_steam_server.Services
{
    public class ResourcesService : IResources
    {
        private readonly string RepositoryPath;
        private readonly IGamesStaticFilesRepository _gamesStaticFilesRepository;

        public ResourcesService(IGamesStaticFilesRepository gamesStaticFilesRepository)
        {
            RepositoryPath = Path.Combine(Directory.GetCurrentDirectory(), "resoures");
            _gamesStaticFilesRepository = gamesStaticFilesRepository;
        }

        public async Task<Stream?> GetLibsStaticFiles(long GameId)
        {
            return await _gamesStaticFilesRepository.GetGameFileAsync((int)GameId);
        }

        public Task<Stream?> GetMarkdownStreamAsync(string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                return Task.FromResult<Stream?>(null);

            var markDownRepositoryPath = Path.Combine(RepositoryPath, "MarkdownFiles");
            var filePath = Path.Combine(markDownRepositoryPath, FileName);

            if(!File.Exists(filePath))
                return Task.FromResult<Stream?>(null);

            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult<Stream?>(stream);
        }
    }
}
