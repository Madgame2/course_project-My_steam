using My_steam_client.Scripts.Interfaces;
using My_steam_client.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Net;
using Microsoft.Extensions.DependencyInjection;
using My_steam_client.Scripts.Models;
using System.IO.Compression;
using Game_Net_DTOLib;

namespace My_steam_client.Scripts
{
    public class LibMannager
    {
        public event Action<long, long>? UnpackProgressChanged;
        public event Action<long>? UnpackCompleted;
        public event Action<long, Exception>? UnpackFailed;

        private readonly string[] _executableExtensions = { ".exe", ".bat", ".cmd" };

        public string manifestFilePath { get; private set; }
        public string LibRootPath {  get; private set; }
        public bool isOfflineMode { get; set; } = false;

        private readonly HashSet<string> _execExt = new(StringComparer.OrdinalIgnoreCase)
    { ".exe", ".bat", ".cmd" /* и т.д. */ };

        public LibRepository repository { get; private set; } = new LibRepository();
        public DownloadQueueManager downloadQueueManager;
        public LibraryService libraryService;
        public LibMannager()
        {
            libraryService = AppServices.Provider.GetRequiredService<LibraryService>();
            downloadQueueManager = AppServices.Provider.GetRequiredService<DownloadQueueManager>();
            LibRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Common");
            manifestFilePath = Path.Combine(LibRootPath, "Lib/LibInit.json");

            if (!isLibInited()) initLib();
            checkLibStuct();

            LibRepository.ManifestFilePath = manifestFilePath;

            downloadQueueManager.DownloadCompleted += UnPacageGame;
        }

        public async void SynnchronizeLibs()
        {
            var DetachedLib = await libraryService.GerDetachedLib(AppServices.UserId.ToString());
            var localLib = await repository.GetAllRecordsAsync();

            foreach (var libItem in DetachedLib)
            {
                var localgameInfo = localLib.Where(p=>p.GameId == libItem.GameId).FirstOrDefault();

                if (localgameInfo==null)
                {
                    createNewRecord(libItem);
                }
                else
                {
                    ChekcTheecord(localgameInfo, libItem);
                }
            }

            await repository.saveChanges(localLib);
        }

        private void checkStaticLibResources(List<ManifestRecord> recors)
        {
            foreach (var record in recors)
            {
                if(record.LibIconSource == null)
                {

                }
            }
        }

        private void ChekcTheecord(ManifestRecord manifestRecord,SynchronizeLibDto dto)
        {
            if (!manifestRecord.UserId.Contains(Convert.ToInt64(dto.UserId)))
            {   
                manifestRecord.UserId.Add(Convert.ToInt64(dto.UserId));
            }

            if (manifestRecord.GameName != dto.Gamename)
            {
                manifestRecord.GameName = dto.Gamename;
            }

            if (manifestRecord.DownloadSource != dto.DownloadSource)
            {
                manifestRecord.DownloadSource = dto.DownloadSource;
            }

            if (manifestRecord.SpaceRequered != dto.SpaceRequered)
            {
                manifestRecord.SpaceRequered = dto.SpaceRequered;
            }
        }
        private ManifestRecord createNewRecord(SynchronizeLibDto dto)
        {
            return new ManifestRecord
            {
                UserId = new List<long> { AppServices.UserId },
                GameId = dto.GameId,
                GameName = dto.Gamename,
                DownloadSource = dto.DownloadSource,
                SpaceRequered = dto.SpaceRequered,
                lastPlayed = DateTime.UtcNow,
            };
        }

        public async void DeleteGame(ManifestRecord manifestRecord)
        {
            try
            {
                var gameDir = Path.Combine(LibRootPath, "Lib", manifestRecord.GameName.Replace(":", "-"));
                
                if (Directory.Exists(gameDir))
                {
                    Directory.Delete(gameDir, true);
                }

                manifestRecord.libElemStaus = LibElemStatuses.Not_instaled;
                manifestRecord.ExecuteFileSource = null;
                await repository.UpdateRecordAsync(manifestRecord.RecordId, manifestRecord);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error deleting game: {ex.Message}");
                throw; 
            }
        }

        public void addToInstalatinQueue(ManifestRecord record)
        {
            var LibPath = Path.Combine(LibRootPath, "Lib");
            var gameDir = Path.Combine(LibPath, record.GameName.Replace(":", "-"));
            var savePath = Path.Combine(gameDir, $"{record.GameName.Replace(":", "-")}.zip");

            if (!Directory.Exists(gameDir)) Directory.CreateDirectory(gameDir);

            downloadQueueManager.EnqueueDownload(record.RecordId, record.DownloadSource, savePath);
        }

        private async void UnPacageGame(DownloadRequest request)
        {
            try
            {
                string ZipFilePath = request.savePath;
                if (!File.Exists(ZipFilePath))
                {
                    throw new FileNotFoundException($"Archive file not found at {ZipFilePath}");
                }

                var extractpath = Path.GetDirectoryName(ZipFilePath);
                string? executablePath = null;

                await Task.Run(() =>
                {
                    using (var archive = ZipFile.OpenRead(ZipFilePath))
                    {
                        long totalEntries = archive.Entries.Count;
                        long processedEntries = 0;

                        foreach (var entry in archive.Entries)
                        {
                            var fullPath = Path.Combine(extractpath, entry.FullName);
                            
                            if (string.IsNullOrEmpty(entry.Name))
                            {
                                Directory.CreateDirectory(fullPath);
                                processedEntries++;
                                UnpackProgressChanged?.Invoke(processedEntries, totalEntries);
                                continue;
                            }

                            var directory = Path.GetDirectoryName(fullPath);
                            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            entry.ExtractToFile(fullPath, true);
                            processedEntries++;
                            UnpackProgressChanged?.Invoke(processedEntries, totalEntries);
                        }
                    }
                });

                File.Delete(ZipFilePath);

                var record = await repository.getRecordByIdAsync(request.ManifestRecordId);
                if (record != null)
                {
                    record.libElemStaus = LibElemStatuses.Installed;
                    
                        executablePath = FindExecutableFile(extractpath);
                        if (executablePath != null)
                        {
                            record.ExecuteFileSource = executablePath;
                        }
                    

                    await repository.UpdateRecordAsync(record.RecordId, record);
                }

                UnpackCompleted?.Invoke(request.ManifestRecordId);
            }
            catch (Exception ex)
            {
                UnpackFailed?.Invoke(request.ManifestRecordId, ex);
                throw;
            }
        }

        private string? FindExecutableFile(string rootDir)
        {
            if (!Directory.Exists(rootDir))
                throw new DirectoryNotFoundException($"Directory not found: {rootDir}");

            try
            {
                var exe = Directory.EnumerateFiles(rootDir, "*.exe", SearchOption.AllDirectories)
                                   .FirstOrDefault();
                if (exe != null)
                    return exe;


                return Directory.EnumerateFiles(rootDir, "*.*", SearchOption.AllDirectories)
                                .FirstOrDefault(f => _execExt.Contains(Path.GetExtension(f)));
            }
            catch (UnauthorizedAccessException uaEx)
            {
                // Логируем и возвращаем null, если нет доступа к какой-то папке
                Console.Error.WriteLine($"Access denied while searching: {uaEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error searching for executable: {ex}");
                return null;
            }
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

        private void checkLibStuct()
        {
            var LibResourses = Path.Combine(LibRootPath, "LidResources");
            if (!Directory.Exists(LibResourses)) Directory.CreateDirectory(LibResourses);
        }
    }
}
