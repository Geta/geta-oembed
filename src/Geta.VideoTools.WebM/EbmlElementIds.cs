// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.VideoTools.WebM;

internal static class EbmlElementIds
{
    public const ulong EbmlHeader = 0x1A45DFA3;
    public const ulong Segment = 0x18538067;
    public const ulong DocType = 0x4282;
    public const ulong DocTypeVersion = 0x4287;
    public const ulong DocTypeReadVersion = 0x4285;
    public const ulong Tracks = 0x1654AE6B;
    public const ulong TrackEntry = 0xAE;
    public const ulong Video = 0xE0;
    public const ulong PixelWidth = 0xB0;
    public const ulong PixelHeight = 0xBA;
}
