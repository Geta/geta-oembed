// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.VideoTools.Common.Streams;

public sealed class SeekableStream : Stream
{
    private readonly byte[] _cache;
    private readonly byte[] _buffer;
    private readonly Stream _baseStream;
    private readonly int _length;
    private int _position;
    private bool _disposed;

    public SeekableStream(Stream stream, int bufferSize = 2048)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Underlying stream must be able to read");

        if (stream.Length > int.MaxValue)
            throw new ArgumentException("Underlying stream cannot be bigger than 2,147,483,647 bytes");

        _cache = new byte[stream.Length];
        _buffer = new byte[bufferSize];
        _baseStream = stream;
        _position = (int)_baseStream.Position;
        _length = (int)_baseStream.Length;
    }

    public override bool CanWrite => _baseStream.CanWrite;
    public override bool CanSeek => true;
    public override bool CanRead => true;
    public override long Length => _baseStream.Length;

    public override long Position
    {
        get => _position; set => Seek(value, SeekOrigin.Begin);
    }

    public override void Flush()
    {
        _baseStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var destination = _position + count;

        RequireFromBase((int)(destination - _baseStream.Position));

        var read = Math.Min(_length - destination, count);

        Buffer.BlockCopy(_cache, _position, buffer, offset, read);

        _position += read;

        return read;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        if (offset > int.MaxValue)
            throw new ArgumentException("Offset cannot be bigger than 2,147,483,647 bytes", nameof(offset));

        int destination;
        int localOffset = (int)offset;

        if (origin == SeekOrigin.End)
        {
            destination = _length - 1 - localOffset;
        }
        else if (origin == SeekOrigin.Current)
        {
            destination = _position + localOffset;
        }
        else
        {
            destination = localOffset;
        }

        RequireFromBase((int)(destination - _baseStream.Position));

        _position = destination;

        return destination;
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _baseStream.Write(buffer, offset, count);

        Buffer.BlockCopy(buffer, offset, _cache, _position, count);

        _position += count;
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

    private int RequireFromBase(int requiredBytes)
    {
        if (requiredBytes <= 0)
            return 0;

        var readTotal = 0;

        do
        {
            var requestBytes = Math.Min(_buffer.Length, requiredBytes);
            var read = _baseStream.Read(_buffer, 0, requestBytes);

            Buffer.BlockCopy(_buffer, 0, _cache, _position, read);

            requiredBytes -= requestBytes;
            readTotal += read;
        }
        while (requiredBytes > 0);

        return readTotal;
    }

    public static Stream Ensure(Stream stream)
    {
        if (stream.CanSeek)
            return stream;

        return new SeekableStream(stream);
    }
}
