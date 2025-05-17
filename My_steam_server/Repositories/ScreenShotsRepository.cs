using My_steam_server.Interfaces;

namespace My_steam_server.Repositories
{
    public class ScreenShotsRepository : IScreenShotsRepository
    {
        private readonly string _repositoryRootPath;

        public ScreenShotsRepository(string repositoryRootPath)
        {
            _repositoryRootPath = repositoryRootPath;
        }

        public async Task<string> saveFileAsync(IFormFile file, string directoryName)
        {
            if (file == null)
                throw new ArgumentException("No files provided", nameof(file));

            var targetDirAbsolute = Path.Combine(_repositoryRootPath, directoryName);


            if (!Directory.Exists(targetDirAbsolute))
                Directory.CreateDirectory(targetDirAbsolute);

            var rootDir = Directory.GetCurrentDirectory();
            var CommonPart = Path.GetRelativePath(rootDir, _repositoryRootPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"ss_{Guid.NewGuid():N}{extension}";
            var fullFilePath = Path.Combine(targetDirAbsolute, fileName);

            // Сохраняем файл
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = Path.Combine(CommonPart, directoryName, fileName).Replace("\\", "/");

            return relativePath;
        }

        public async Task<List<string>> saveFilesAsync(List<IFormFile> files, string directoryName)
        {
            if (files == null || files.Count == 0)
                throw new ArgumentException("No files provided", nameof(files));

            var targetDirAbsolute = Path.Combine(_repositoryRootPath, directoryName);


            if (!Directory.Exists(targetDirAbsolute))
                Directory.CreateDirectory(targetDirAbsolute);


            var relativePaths = new List<string>();


            var rootDir = Directory.GetCurrentDirectory();
            var CommonPart = Path.GetRelativePath(rootDir, _repositoryRootPath);
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"ss_{Guid.NewGuid():N}{extension}";
                var fullFilePath = Path.Combine(targetDirAbsolute, fileName);

                // Сохраняем файл
                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = Path.Combine(CommonPart, directoryName, fileName).Replace("\\", "/");
                relativePaths.Add(relativePath);
            }

            return relativePaths;
        }
    }
}
