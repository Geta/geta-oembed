// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.VideoTools.Common;

public interface IStreamParser<out TData>
{
    /// <summary>
    /// Reads video metadata from stream, will consume the stream.
    /// </summary>
    /// <param name="stream">Input stream (must be seekable)</param>
    /// <returns>TData</returns>
    public TData? Parse(Stream stream);
}
