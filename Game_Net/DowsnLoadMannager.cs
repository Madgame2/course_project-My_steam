using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    public enum DownloadStatus
    {
        Queued,
        Downloading,
        Paused,
        Completed,
        Failed
    }

    public class DownloadProgress
    {
        public long BytesDownloaded { get; set; }
        public long TotalBytes { get; set; }
        public DownloadStatus Status { get; set; }
        public Exception? Error { get; set; }
    }

    public class DownloadQueueManager
    {
        private readonly ComunitationMannageer _commManager;
        private readonly ConcurrentQueue<DownloadRequest> _queue = new();
        private DownloadTask? _currentTask;
        private bool _isProcessing = false;
        private readonly Dictionary<long, DownloadProgress> _downloadProgress = new();

        public event Action<DownloadRequest>? DownloadCompleted;
        public event Action<DownloadRequest, DownloadProgress>? DownloadProgressChanged;
        public event Action<DownloadRequest>? DownloadFailed;

        public DownloadQueueManager(ComunitationMannageer commManager)
        {
            _commManager = commManager;
        }

        public void EnqueueDownload(long gameId, string url, string savePath)
        {
            var request = new DownloadRequest(gameId, url, savePath);
            _queue.Enqueue(request);
            _downloadProgress[gameId] = new DownloadProgress 
            { 
                Status = DownloadStatus.Queued,
                BytesDownloaded = 0,
                TotalBytes = 0
            };
            StartNextIfIdle();
        }

        public void PauseCurrent()
        {
            if (_currentTask != null)
            {
                _currentTask.Pause();
                var gameId = _currentTask._request.ManifestRecordId;
                if (_downloadProgress.ContainsKey(gameId))
                {
                    _downloadProgress[gameId].Status = DownloadStatus.Paused;
                    DownloadProgressChanged?.Invoke(_currentTask._request, _downloadProgress[gameId]);
                }
            }
        }

        public void ResumeCurrent()
        {
            if (_currentTask != null)
            {
                _currentTask.Resume();
                var gameId = _currentTask._request.ManifestRecordId;
                if (_downloadProgress.ContainsKey(gameId))
                {
                    _downloadProgress[gameId].Status = DownloadStatus.Downloading;
                    DownloadProgressChanged?.Invoke(_currentTask._request, _downloadProgress[gameId]);
                }
            }
        }

        public void CancelCurrent()
        {
            _currentTask?.Cancel();
        }

        public DownloadProgress? GetDownloadProgress(long gameId)
        {
            return _downloadProgress.TryGetValue(gameId, out var progress) ? progress : null;
        }

        private void StartNextIfIdle()
        {
            if (_isProcessing) return;

            Task.Run(async () =>
            {
                while (_queue.TryDequeue(out var next))
                {
                    try
                    {
                        _isProcessing = true;
                        _currentTask = new DownloadTask(next, _commManager);
                        
                        var gameId = next.ManifestRecordId;
                        if (_downloadProgress.ContainsKey(gameId))
                        {
                            _downloadProgress[gameId].Status = DownloadStatus.Downloading;
                            DownloadProgressChanged?.Invoke(next, _downloadProgress[gameId]);
                        }

                        await _currentTask.StartAsync();
                        
                        if (_downloadProgress.ContainsKey(gameId))
                        {
                            _downloadProgress[gameId].Status = DownloadStatus.Completed;
                            DownloadProgressChanged?.Invoke(next, _downloadProgress[gameId]);
                        }
                        
                        DownloadCompleted?.Invoke(next);
                    }
                    catch (Exception ex)
                    {
                        var gameId = next.ManifestRecordId;
                        if (_downloadProgress.ContainsKey(gameId))
                        {
                            _downloadProgress[gameId].Status = DownloadStatus.Failed;
                            _downloadProgress[gameId].Error = ex;
                            DownloadProgressChanged?.Invoke(next, _downloadProgress[gameId]);
                        }
                        DownloadFailed?.Invoke(next);
                    }
                    finally
                    {
                        _currentTask = null;
                    }
                }

                _isProcessing = false;
            });
        }
    }
}
