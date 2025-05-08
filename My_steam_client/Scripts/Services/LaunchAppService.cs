using My_steam_client.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task LaunchAndTrackGame(long libRecordId,string gameName, string gamePath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = gamePath,
                    UseShellExecute = true
                };

                var gameProcess = Process.Start(startInfo);
                var startTime = DateTime.Now;

                if (gameProcess == null) return;
                var session = new GameSession
                {
                    RecordId = libRecordId,
                    Name = gameName,
                    Process = gameProcess,
                    StartTime = startTime
                };

                _sessions.Add(session);

                await Task.Run(async () =>
                {
                    gameProcess.WaitForExit();

                    DateTime endTime = DateTime.Now;
                    TimeSpan duration = endTime - session.StartTime;

                    var oldRecord = await _libRepository.getRecordByIdAsync(session.RecordId);
                    oldRecord.playedTime += duration;
                    await _libRepository.UpdateRecordAsync(session.RecordId, oldRecord);

                    _sessions.Remove(session);
                });
            }
            catch (Exception ex) { 
                Debug.WriteLine(ex.Message);
            }
        }
        public async Task TerminateGameAsync(long libRecordId)
        {
            var session = _sessions.FirstOrDefault(p => p.RecordId == libRecordId);
            if (session == null) return;

            var process = session.Process;
            process.Kill();

            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - session.StartTime;

            var oldRecord = await _libRepository.getRecordByIdAsync(libRecordId);
            if (oldRecord == null) return;

            oldRecord.playedTime += duration;
            await _libRepository.UpdateRecordAsync(session.RecordId, oldRecord);

            _sessions.Remove(session);
        }

    }
}
