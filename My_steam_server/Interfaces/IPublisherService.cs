using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IPublisherService
    {
        public Dictionary<Guid, Game> Processed_goods { get; set; }

        Task<string> DeployGameFiles(Stream stream, long GameId);

        Task<List<string>> DeployScreenShotsFiles(List<IFormFile> Files, string GameName);

        Task<string> DeployHeaderImageAsync(IFormFile stream, string GameName);

        Task DeployLibImages(IFormFile LibIcon, IFormFile LibHeader, long gameId);
    }
}
