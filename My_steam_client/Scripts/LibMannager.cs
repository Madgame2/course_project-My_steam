using My_steam_client.Scripts.Interfaces;
using My_steam_client.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Scripts
{
    public class LibMannager
    {
        public string manifestFilePath { get; private set; }

        public bool isOfflineMode { get; set; } = false;

        public LibRepository repository { get; private set; } = new LibRepository();
        public LibMannager()
        {
            var CommonPath = Path.Combine(Directory.GetCurrentDirectory(), "Common");
            manifestFilePath = Path.Combine(CommonPath, "Lib/LibInit.json");

            if (!isLibInited()) initLib();

            LibRepository.ManifestFilePath = manifestFilePath;
        }

        private bool isLibInited()
        {
           return File.Exists(manifestFilePath);
        }

        private void initLib()
        {
            var directoryPath = Path.GetDirectoryName(manifestFilePath);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            else
            {
                throw new InvalidOperationException("Путь к директории манифеста недопустим.");
            }

            using var stream = File.Create(manifestFilePath);
            using var writer = new StreamWriter(stream);
            writer.Write("[]");
            stream.Flush();
        }

        public async Task<List<LibraryListItem>> getLibAsync()
        {
            var objeccts = await repository.getRecordsByUserIdAsync(AppServices.UserId);

            var list = new List<LibraryListItem>();
            foreach (var item in objeccts) { 
                var newItem = new LibraryListItem();

                newItem.id = item.GameId;
                newItem.GameName = item.GameName;
                newItem.ImageLink = item.LibIconSource;
                newItem.RecordId = item.RecordId;

                list.Add(newItem);
            }

            return list;
        }
    }
}
