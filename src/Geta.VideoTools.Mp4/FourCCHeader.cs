// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.VideoTools.Mp4;

public sealed record FourCCHeader
{
    public FourCCHeader(string ftyp)
    {
        FType = ftyp;
    }

    public string FType { get; }
}
