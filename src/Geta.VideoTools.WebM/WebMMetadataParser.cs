// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.VideoTools.Common;
using Geta.VideoTools.Common.Models;
using Geta.VideoTools.Common.Streams;

namespace Geta.VideoTools.WebM;

public sealed class WebMMetadataParser : IStreamParser<VideoMeta>
{
    /// <summary>
    /// Reads video metadata from stream, will consume the stream.
    /// </summary>
    /// <param name="stream">Input stream (must be seekable)</param>
    /// <returns>Video metadata</returns>
    public VideoMeta? Parse(Stream stream)
    {
        stream = SeekableStream.Ensure(stream);

        var reader = GetEbmlReader(stream);

        try
        {
            var header = ReadEbmlHeader(reader);
            if (header is null)
                return null;

            if (!Constants.DocTypeDesignations.Contains(header.DocType))
                return null;

            return ReadMetadata(reader);
        }
        finally
        {
            reader.Dispose();
        }
    }

    private static VideoMeta? ReadMetadata(EbmlReader reader)
    {
        VideoMeta? metadata = null;

        while (reader.ReadNext())
        {
            switch (reader.ElementId.EncodedValue)
            {
                case EbmlElementIds.Segment:
                case EbmlElementIds.Tracks:
                case EbmlElementIds.TrackEntry: reader.EnterContainer(); break;
                case EbmlElementIds.Video: metadata = ReadVideoElement(reader); break;
            }

            if (metadata is not null)
                return metadata;
        }

        return metadata;
    }

    private static VideoMeta? ReadVideoElement(EbmlReader reader)
    {
        var width = (ulong?)null;
        var height = (ulong?)null;

        reader.EnterContainer();

        do
        {
            reader.ReadNext();

            switch (reader.ElementId.EncodedValue)
            {
                case EbmlElementIds.PixelWidth: width = reader.ReadUInt(); break;
                case EbmlElementIds.PixelHeight: height = reader.ReadUInt(); break;
            }
        }
        while (!width.HasValue || !height.HasValue);

        reader.LeaveContainer();

        return new VideoMeta
        {
            Width = (int)width.Value,
            Height = (int)height.Value,
        };
    }

    private static EbmlHeader? ReadEbmlHeader(EbmlReader reader)
    {
        if (!reader.ReadNext())
            return null;

        if (reader.ElementId != EbmlElements.EbmlHeader)
            return null;

        reader.EnterContainer();

        do
        {
            reader.ReadNext();
        }
        while (reader.ElementId != EbmlElements.DocType);

        var docType = reader.ReadAscii();

        reader.LeaveContainer();

        return new EbmlHeader(docType);
    }

    private static EbmlReader GetEbmlReader(Stream stream)
    {
        return new EbmlReader(stream);
    }
}
