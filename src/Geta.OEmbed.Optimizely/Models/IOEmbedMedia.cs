// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Core;

namespace Geta.OEmbed.Optimizely.Models
{
    public interface IOEmbedMedia : IContentMedia
    {
        string? Title { get; }
        int? Width { get; }
        int? Height { get; }
    }
}
