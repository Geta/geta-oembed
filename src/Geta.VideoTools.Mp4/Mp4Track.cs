// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.VideoTools.Mp4;

[Flags]
public enum Mp4Track
{
    None = 0x0000,
    Enabled = 0x0001,
    UsedInMovie = 0x0002,
    UsedInPreview = 0x0004,
    UsedInPoster = 0x0008
}
