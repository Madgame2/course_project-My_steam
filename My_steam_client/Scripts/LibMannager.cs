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
using My_steam_server.Interfaces;
using System.Windows;

namespace My_steam_client.Scripts
{
    public class LibMannager
    {
        public delegate void LibContains(List<ManifestRecord> localLib);

        public event Action<long, long>? UnpackProgressChanged;
        public event Action<long>? UnpackCompleted;
        public event Action<long, Exception>? UnpackFailed;

        public event LibContains LibResynchronized;

        private readonly string[] _executableExtensions = { ".exe", ".bat", ".cmd" };

        public string manifestFilePath { get; private set; }
        public string LibRootPath {  get; private set; }
        public string ResureDirectoryPath {  get; private set; }
        public bool isOfflineMode { get; set; } = false;

        private readonly HashSet<string> _execExt = new(StringComparer.OrdinalIgnoreCase)
    { ".exe", ".bat", ".cmd" /* и т.д. */ };

        public LibRepository repository { get; private set; } = new LibRepository();
        public DownloadQueueManager downloadQueueManager;
        private LibraryService libraryService;
        private DownloadingLibStaticResourcesService downloadingLibStaticResourcesService;
        public LibMannager()
        {
            downloadingLibStaticResourcesService = AppServices.Provider.GetRequiredService<DownloadingLibStaticResourcesService>();
            libraryService = AppServices.Provider.GetRequiredService<LibraryService>();
            downloadQueueManager = AppServices.Provider.GetRequiredService<DownloadQueueManager>();
            LibRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Common");
            manifestFilePath = Path.Combine(LibRootPath, "Lib/LibInit.json");
            ResureDirectoryPath = Path.Combine(LibRootPath, "LidResources");

            if (!isLibInited()) initLib();
            checkLibStuct();

            LibRepository.ManifestFilePath = manifestFilePath;

            checkGamesFiles();

            downloadQueueManager.DownloadCompleted += UnPacageGame;
        }

        public async void SynnchronizeLibs()
        {
            var DetachedLib = await libraryService.GerDetachedLib(AppServices.UserId.ToString());
            var localLib = await repository.GetAllRecordsAsync();


            
            foreach (var libItem in DetachedLib)
            {
                var localgameInfo = localLib.Where(p=>p.GameId == libItem.GameId).FirstOrDefault();

                if (localgameInfo==null||localgameInfo.UserId!= AppServices.UserId)
                {
                    localLib.Add(createNewRecord(libItem));
                }
                else
                {
                    ChekcRheecord(localgameInfo, libItem);
                }
            }

            await checkStaticLibResources(localLib);
            await CheckGamesFiles(localLib);
            await repository.saveChanges(localLib);

            LibResynchronized?.Invoke(localLib);
        }
        public async Task<List<LibraryListItem>> getLibAsync()
        {
            var objeccts = await repository.getRecordsByUserIdAsync(AppServices.UserId);

            var list = new List<LibraryListItem>();
            foreach (var item in objeccts)
            {
                var newItem = new LibraryListItem();

                newItem.id = item.GameId;
                newItem.GameName = item.GameName;
                newItem.ImageLink = item.LibIconSource;
                newItem.RecordId = item.RecordId;

                list.Add(newItem);
            }

            return list;
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
        public async Task<DateTime?> GetLastPurchase()
        {
            var libItemms= await repository.getRecordsByUserIdAsync(AppServices.UserId);
            return libItemms.OrderByDescending(e => e.PurhcaseDate).FirstOrDefault()?.PurhcaseDate;
        }
        public async Task<int> GetGamesCount()
        {
            return (await repository.getRecordsByUserIdAsync(AppServices.UserId)).Length;
        }
        public async Task<TimeSpan> GetTimeInGames()
        {
            var lib = await repository.getRecordsByUserIdAsync(AppServices.UserId);

            var time = TimeSpan.Zero;

            foreach (var record in lib)
            {
                time += record.playedTime;
            }

            return time;
        }

        private Task CheckGamesFiles(List<ManifestRecord> records)
        {
            var LibPath = Path.Combine(LibRootPath, "Lib");


            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                var directoryPath = Path.Combine(LibPath, record.GameName);

                if (string.IsNullOrEmpty(record.ExecuteFileSource) && Directory.Exists(directoryPath))
                {
                    var execPath = FindExecutableFile(directoryPath);

                    if (execPath != null)
                    {
                        record.ExecuteFileSource = execPath;
                        record.libElemStaus = LibElemStatuses.Installed;
                    }
                }
            }

            return Task.CompletedTask;
        }
        private async void checkGamesFiles()
        {
            var ManifestRecords = await repository.GetAllRecordsAsync();

            for (int i = 0; i < ManifestRecords.Count; i++)
            {

                var record = ManifestRecords[i];

                if (!string.IsNullOrEmpty(record.ExecuteFileSource) && !File.Exists(record.ExecuteFileSource))
                {
                    record.ExecuteFileSource = null;
                    record.libElemStaus = LibElemStatuses.Not_instaled;
                }
            }
            await repository.saveChanges(ManifestRecords);

        }
        private async Task checkStaticLibResources(List<ManifestRecord> records)
        {
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];

                if (record.LibIconSource == null || record.HeaderImageSource == null|| !File.Exists(record.LibIconSource)|| !File.Exists(record.HeaderImageSource))
                {
                    var invalidChars = Path.GetInvalidFileNameChars();
                    var safeGameName = string.Concat(record.GameName.Where(c => !invalidChars.Contains(c)));
                    var localGameresourcesPath = Path.Combine(ResureDirectoryPath, safeGameName);

                    // Создаём директорию, если её нет
                    if (!Directory.Exists(localGameresourcesPath))
                        Directory.CreateDirectory(localGameresourcesPath);

                    // Очищаем директорию от файлов
                    if (Directory.Exists(localGameresourcesPath))
                    {
                        foreach (string file in Directory.GetFiles(localGameresourcesPath))
                        {
                            File.Delete(file);
                        }
                    }

                    try
                    {
                        // Скачиваем и распаковываем архив
                        await downloadingLibStaticResourcesService.InstallArchiveAsync(localGameresourcesPath, record.GameId);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load static resources: {record.GameName}");
                        continue;
                    }
                    // Поиск и установка путей для изображений
                    foreach (string file in Directory.GetFiles(localGameresourcesPath))
                    {
                        string fileName = Path.GetFileName(file);

                        if (fileName.Contains("icon"))
                        {
                            record.LibIconSource = file;
                        }
                        else if (fileName.Contains("head"))
                        {
                            record.HeaderImageSource = file;
                        }
                    }
                }
            }
        }
        private void ChekcRheecord(ManifestRecord manifestRecord,SynchronizeLibDto dto)
        {
            //if (!manifestRecord.UserId.Contains(Convert.ToInt64(dto.UserId)))
            //{   
            //    manifestRecord.UserId.Add(Convert.ToInt64(dto.UserId));
            //}

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

            if (manifestRecord.PurhcaseDate != dto.PurchaseDate)
            {
                manifestRecord.PurhcaseDate = dto.PurchaseDate;
            }
        }
        private ManifestRecord createNewRecord(SynchronizeLibDto dto)
        {
            return new ManifestRecord
            {
                UserId = AppServices.UserId,
                GameId = dto.GameId,
                GameName = dto.Gamename,
                DownloadSource = dto.DownloadSource,
                SpaceRequered = dto.SpaceRequered,
                lastPlayed = DateTime.UtcNow,
                PurhcaseDate = dto.PurchaseDate
            };
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
        private void checkLibStuct()
        {
            var LibResourses = Path.Combine(LibRootPath, "LidResources");
            if (!Directory.Exists(LibResourses)) Directory.CreateDirectory(LibResourses);
        }

    }
}
