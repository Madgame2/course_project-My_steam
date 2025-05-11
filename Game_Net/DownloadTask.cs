using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Game_Net
{
    public class DownloadTask
    {
        public readonly DownloadRequest _request;
        private readonly ComunitationMannageer _commManager;

        private CancellationTokenSource _cts = new();
        private ManualResetEventSlim _pauseEvent = new(true);

        public bool IsCompleted { get; private set; } = false;
        public Exception? LastError { get; private set; }
        public long TotalBytes { get; private set; }
        public long BytesDownloaded { get; private set; }

        public event Action<long, long>? ProgressChanged;

        public DownloadTask(DownloadRequest request, ComunitationMannageer commManager)
        {
            _request = request;
            _commManager = commManager;
        }

        public async Task StartAsync()
        {
            try
            {
                var directory = Path.GetDirectoryName(_request.savePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using var stream = await _commManager.GetResourcesAsyc(_request.URL);
                if (stream == null)
                {
                    throw new Exception("Failed to get download stream from server");
                }

                if (stream.CanSeek)
                {
                    TotalBytes = stream.Length;
                }

                using var fileStream = new FileStream(_request.savePath, FileMode.Create, FileAccess.Write, FileShare.None);
                byte[] buffer = new byte[81920];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, _cts.Token)) > 0)
                {
                    _pauseEvent.Wait(_cts.Token);
                    await fileStream.WriteAsync(buffer, 0, bytesRead, _cts.Token);
                    BytesDownloaded += bytesRead;
                    ProgressChanged?.Invoke(BytesDownloaded, TotalBytes);
                }

                await fileStream.FlushAsync();
                IsCompleted = true;
            }
            catch (OperationCanceledException)
            {
                LastError = new Exception("Download was cancelled");
                throw;
            }
            catch (Exception ex)
            {
                LastError = ex;
                throw;
            }
        }

        public void Pause() => _pauseEvent.Reset();
        public void Resume() => _pauseEvent.Set();
        public void Cancel() => _cts.Cancel();
    }
}
