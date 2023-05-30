// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.VideoTools.WebM;

internal static class EbmlElements
{
    public static readonly VInt EbmlHeader = new(EbmlElementIds.EbmlHeader, 4);
    public static readonly VInt DocType = new(EbmlElementIds.DocType, 2);
    public static readonly VInt DocTypeVersion = new(EbmlElementIds.DocTypeVersion, 2);
    public static readonly VInt DocTypeReadVersion = new(EbmlElementIds.DocTypeReadVersion, 2);

    public static readonly VInt Segment = new(EbmlElementIds.Segment, 4);
    public static readonly VInt Tracks = new(EbmlElementIds.Tracks, 4);
    public static readonly VInt TrackEntry = new(EbmlElementIds.TrackEntry, 1);
    public static readonly VInt Video = new(EbmlElementIds.Video, 1);
    public static readonly VInt PixelWidth = new(EbmlElementIds.PixelWidth, 1);
    public static readonly VInt PixelHeight = new(EbmlElementIds.PixelHeight, 1);
}
