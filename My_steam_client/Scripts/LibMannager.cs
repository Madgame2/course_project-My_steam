using My_steam_client.Scripts.Interfaces;
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

        private LibRepository repository = new LibRepository();
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

            File.Create(manifestFilePath).Dispose();
        }
    }
}
