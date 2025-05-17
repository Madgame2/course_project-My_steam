namespace My_steam_server
{
    public class ChunkedReadStream : Stream
    {
        private readonly IEnumerator<string> _chunkEnumerator;
        private FileStream _currentChunkStream;

        public ChunkedReadStream(IEnumerable<string> chunkFiles)
        {
            _chunkEnumerator = chunkFiles.GetEnumerator();
            MoveToNextChunk();
        }

        private void MoveToNextChunk()
        {
            _currentChunkStream?.Dispose();
            if (_chunkEnumerator.MoveNext())
            {
                _currentChunkStream = File.OpenRead(_chunkEnumerator.Current);
            }
            else
            {
                _currentChunkStream = null;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_currentChunkStream == null) return 0;

            int bytesRead = _currentChunkStream.Read(buffer, offset, count);
            if (bytesRead == 0)
            {
                MoveToNextChunk();
                return Read(buffer, offset, count); // рекурсивно переходим к следующему чанку
            }

            return bytesRead;
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            _currentChunkStream?.Dispose();
            _chunkEnumerator?.Dispose();
            base.Dispose(disposing);
        }
    }
}
