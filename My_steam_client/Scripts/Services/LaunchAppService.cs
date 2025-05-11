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
                Debug.WriteLine($"Attempting to launch: {gamePath}");

                if (!File.Exists(gamePath))
                {
                    throw new FileNotFoundException($"Game executable not found at: {gamePath}");
                }

                var gameDirectory = Path.GetDirectoryName(gamePath);
                if (string.IsNullOrEmpty(gameDirectory))
                {
                    throw new InvalidOperationException("Invalid game directory");
                }

                // Создаем ProcessStartInfo с базовыми настройками
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = gamePath,
                    WorkingDirectory = gameDirectory,
                    UseShellExecute = true,
                    Verb = "open"
                };

                // Пробуем запустить процесс
                Process? gameProcess = null;
                try
                {
                    gameProcess = Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"First launch attempt failed: {ex.Message}");
                    
                    // Если первый способ не сработал, пробуем альтернативный способ
                    startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c start \"\" \"{gamePath}\"",
                        UseShellExecute = true,
                        WorkingDirectory = gameDirectory,
                        CreateNoWindow = true
                    };
                    
                    gameProcess = Process.Start(startInfo);
                }

                if (gameProcess == null)
                {
                    throw new Exception("Failed to start game process");
                }

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
