using My_steam_server.Interfaces;
using System.ComponentModel.Design;

namespace My_steam_server.Services
{
    public class ResourcesService : IResources
    {
        private readonly string RepositoryPath;

        public ResourcesService()
        {
            RepositoryPath = Path.Combine(Directory.GetCurrentDirectory(), "resoures");
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
