using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Game_Net
{
    public class DownloadingLibStaticResourcesService
    {
        private readonly ComunitationMannageer comunitationMannageer;

        public DownloadingLibStaticResourcesService(ComunitationMannageer comunitationMannageer)
        {
            this.comunitationMannageer = comunitationMannageer;
        }


        public async Task InstallArchiveAsync(string directoryPath, long gameId)
        {
            // 1. Убедимся, что папка существует
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 2. Скачиваем архив как поток
            Stream? stream = await comunitationMannageer.GetResourcesAsyc($"Resources/Game/{gameId}", Protocol.Http);

            if (stream == null)
            {
                throw new FileNotFoundException($"Не удалось получить архив для игры с ID {gameId}");
            }

            // 3. Временное имя архива внутри папки
            string archivePath = Path.Combine(directoryPath, $"game_{gameId}.zip");

            // 4. Сохраняем архив
            using (var fileStream = new FileStream(archivePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await stream.CopyToAsync(fileStream);
            }

            // 5. Распаковываем в ту же папку
            ZipFile.ExtractToDirectory(archivePath, directoryPath, overwriteFiles: true);

            // 6. Удаляем архив
            File.Delete(archivePath);
        }

    }
}
