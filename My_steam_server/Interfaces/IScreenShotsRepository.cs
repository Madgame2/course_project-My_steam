using My_steam_server.Models;

namespace My_steam_server.Interfaces
{
    public interface IScreenShotsRepository
    {
        Task<List<string>> saveFilesAsync(List<IFormFile> files, string directoryName);
        Task<string> saveFileAsync(IFormFile files, string directoryName);
    }
}
