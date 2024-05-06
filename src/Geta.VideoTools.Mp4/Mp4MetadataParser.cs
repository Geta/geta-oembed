// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.VideoTools.Common;
using Geta.VideoTools.Common.Extensions;
using Geta.VideoTools.Common.Models;
using Geta.VideoTools.Common.Readers;
using Geta.VideoTools.Common.Streams;

namespace Geta.VideoTools.Mp4;

public sealed class Mp4MetadataParser : IStreamParser<VideoMeta>
{
    private const int _atomHeaderSize = 8;

    /// <summary>
    /// Reads video metadata from stream, will consume the stream.
    /// </summary>
    /// <param name="stream">Input stream (must be seekable)</param>
    /// <returns>Video metadata</returns>
    public VideoMeta? Parse(Stream stream)
    {
        stream = SeekableStream.Ensure(stream);

        var reader = GetBinaryReader(stream);
        var atomLength = (int)default;
        var atomName = string.Empty;

        try
        {
            if (!ReadNextAtom(reader, ref atomLength, ref atomName))
                return null;

            var header = ReadFourCCHeader(reader, atomLength, atomName);
            if (header is null)
                return null;

            if (!Constants.FTypeDesignations.Contains(header.FType))
                return null;

            return ReadMetadata(reader);
        }
        finally
        {
            reader.Dispose();
        }
    }

    private static BinaryReader GetBinaryReader(Stream stream)
    {
        if (BitConverter.IsLittleEndian)
            return new LittleEndianBinaryReader(stream);

        return new BinaryReader(stream);
    }

    private static bool ReadNextAtom(BinaryReader reader, ref int length, ref string name)
    {
        var stream = reader.BaseStream;
        var remainingBytes = stream.Length - stream.Position;

        if (_atomHeaderSize >= remainingBytes)
            return false;

        length = reader.ReadInt32() - _atomHeaderSize;

        if (length > remainingBytes)
            return false;

        name = reader.ReadString(4);

        return true;
    }

    private static FourCCHeader? ReadFourCCHeader(BinaryReader reader, int length, string name)
    {
        if (name != Constants.FTypeIndicator)
            return null;

        var ftyp = reader.ReadString(4);
        reader.BaseStream.Seek(length - 4, SeekOrigin.Current);

        return new FourCCHeader(ftyp);
    }

    private static VideoMeta? ReadMetadata(BinaryReader reader)
    {
        int atomLength = default;
        string atomName = string.Empty;

        VideoMeta? metadata = null;

        while (ReadNextAtom(reader, ref atomLength, ref atomName))
        {
            var stream = reader.BaseStream;
            var startPosition = stream.Position;
            var endPosition = startPosition + atomLength;

            switch (atomName)
            {
                case "moov":
                case "trak": metadata = ReadMetadata(reader); break;
                case "tkhd": metadata = ReadTrackHeader(reader, atomLength); break;
                case "mdat": SkipAtom(stream, atomLength); break;
            }

            if (metadata is not null)
                return metadata;

            if (stream.Position > endPosition)
                throw new InvalidOperationException("Stream was overconsumed");

            if (stream.Position < endPosition)
            {
                stream.Seek(endPosition - stream.Position, SeekOrigin.Current);
            }
        }

        return metadata;
    }

    private static void SkipAtom(Stream stream, int length)
    {
        stream.Seek(length, SeekOrigin.Current);
    }

    private static VideoMeta? ReadTrackHeader(BinaryReader reader, int length)
    {
        var stream = reader.BaseStream;
        var startPosition = stream.Position;

        _ = reader.ReadByte(); // Version
        _ = reader.ReadBytes(3); // Flags
        _ = reader.ReadUInt32(); // Creation time
        _ = reader.ReadUInt32(); // Modification time
        _ = reader.ReadUInt32(); // TrackId
        _ = reader.ReadUInt32(); // Reserved
        _ = reader.ReadUInt32(); // Duration
        _ = reader.ReadUInt64(); // Reserved
        _ = reader.ReadUInt16(); // Layer
        _ = reader.ReadUInt16(); // Alternate group
        _ = reader.ReadUInt16(); // Volume
        _ = reader.ReadBytes(36); // Matrix structure

        var trackWidth = reader.ReadUInt32();
        var trackHeight = reader.ReadUInt32();

        var endPosition = stream.Position;
        var offset = endPosition - startPosition;

        if (offset < length)
            stream.Seek(length - offset, SeekOrigin.Current);

        if (trackWidth == default && trackHeight == default)
            return null;

        return new VideoMeta
        {
            Width = (int)trackWidth,
            Height = (int)trackHeight,
        };
    }
}
