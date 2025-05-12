using My_steam_client.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace My_steam_client.Scripts.Services
{
    class GameSession
    {
        public long RecordId { get; set; }
        public string Name { get; set; }
        public Process Process { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class LaunchAppService
    {
        private List<GameSession> _sessions = new List<GameSession>();
        private ILibRepository _libRepository;

        public LaunchAppService(ILibRepository libRepository)
        {
            _libRepository = libRepository;
        }
        ~LaunchAppService()
        {

        }
        public async Task<TimeSpan?> LaunchAndTrackGame(long libRecordId, string gameName, string gamePath)
        {
            try
            {

                if (!File.Exists(gamePath))
                {
                    throw new FileNotFoundException($"Game executable not found at: {gamePath}");
                }

                var gameDirectory = Path.GetDirectoryName(gamePath);
                if (string.IsNullOrEmpty(gameDirectory))
                {
                    throw new InvalidOperationException("Invalid game directory");
                }

                var exeFolder = Path.GetDirectoryName(gamePath)!;
                
                var gameRoot = Directory.Exists(Path.Combine(exeFolder, "Config"))
                    ? exeFolder
                    : 
                      Path.GetFullPath(Path.Combine(exeFolder, "..", "..", ".."));

                var startInfo = new ProcessStartInfo
                {
                    FileName = gamePath,
                    //Arguments = "-log",          // чтобы UE4 писал лог в консоль
                    WorkingDirectory = gameRoot,
                    UseShellExecute = false,          // прямой запуск
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false          // покажем консоль, чтобы увидеть ошибки
                };
                
                var gameProcess = new Process{ StartInfo = startInfo};
                //gameProcess.OutputDataReceived += (s, e) => { if (e.Data != null) Debug.WriteLine(e.Data); };
                //gameProcess.ErrorDataReceived += (s, e) => { if (e.Data != null) Debug.WriteLine(e.Data); };

                gameProcess.Start();
                //gameProcess.BeginOutputReadLine();
                //gameProcess.BeginErrorReadLine();

                var startTime = DateTime.Now;

                var session = new GameSession
                {
                    RecordId = libRecordId,
                    Name = gameName,
                    Process = gameProcess,
                    StartTime = startTime
                };

                _sessions.Add(session);

                var oldRecord = await _libRepository.getRecordByIdAsync(session.RecordId);
                oldRecord.lastPlayed = DateTime.Now;
                await _libRepository.UpdateRecordAsync(session.RecordId, oldRecord);


                // Ждем завершения процесса
                await Task.Run(() => 
                {
                    try
                    {
                        gameProcess.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error waiting for process exit: {ex.Message}");
                    }
                });

                DateTime endTime = DateTime.Now;
                TimeSpan duration = endTime - session.StartTime;

                oldRecord = await _libRepository.getRecordByIdAsync(session.RecordId);
                oldRecord.playedTime += duration;
                await _libRepository.UpdateRecordAsync(session.RecordId, oldRecord);

                _sessions.Remove(session);

                return duration;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error launching game: {ex.Message}");
                throw;
            }
        }


        public async Task<TimeSpan?> TerminateGameAsync(long libRecordId)
        {
            var session = _sessions.FirstOrDefault(p => p.RecordId == libRecordId);
            if (session == null) return null;

            try
            {
                var process = session.Process;
                if (!process.HasExited)
                {
                    try
                    {
                        // Сначала пробуем корректно завершить процесс
                        process.CloseMainWindow();
                        if (!process.WaitForExit(3000)) // Ждем 3 секунды
                        {
                            // Если процесс не завершился, принудительно завершаем
                            process.Kill();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error terminating process: {ex.Message}");
                        process.Kill(); // Принудительное завершение в случае ошибки
                    }
                }

                DateTime endTime = DateTime.Now;
                TimeSpan duration = endTime - session.StartTime;

                var oldRecord = await _libRepository.getRecordByIdAsync(libRecordId);
                if (oldRecord == null) return null;

                oldRecord.playedTime += duration;
                await _libRepository.UpdateRecordAsync(session.RecordId, oldRecord);

                _sessions.Remove(session);

                return duration;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error terminating game: {ex.Message}");
                throw;
            }
        }

    }
}
