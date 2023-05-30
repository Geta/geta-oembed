// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.VideoTools.WebM;

public sealed record EbmlHeader
{
    public EbmlHeader(string type)
    {
        DocType = type;
    }

    public string DocType { get; }
}
