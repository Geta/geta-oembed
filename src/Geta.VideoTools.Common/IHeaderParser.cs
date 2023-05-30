// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.VideoTools.Common;

public interface IHeaderParser<out THeader>
{
    THeader? Parse(ReadOnlySpan<byte> header);
}
