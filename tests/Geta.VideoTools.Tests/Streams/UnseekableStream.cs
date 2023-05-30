namespace Geta.VideoTools.Tests.Streams;

internal class UnseekableStream : Stream
{
    private readonly Stream _baseStream;
    private bool _disposed;

    public UnseekableStream(Stream stream)
    {
        _baseStream = stream;
    }

    public override bool CanRead => _baseStream.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => _baseStream.CanWrite;

    public override long Length => _baseStream.Length;

    public override long Position { get => _baseStream.Position; set => _baseStream.Position = value; }

    public override void Flush()
    {
        _baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _baseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        _baseStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _baseStream.Write(buffer, offset, count);
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _baseStream.Dispose();
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }
}
