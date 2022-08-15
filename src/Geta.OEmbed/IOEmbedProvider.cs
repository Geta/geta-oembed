// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Models;

namespace Geta.OEmbed
{
    public interface IOEmbedProvider
    {
        string ProviderName { get; }
        string ProviderUrl { get; }
        List<OEmbedEndpoint> Endpoints { get; }
        string GetOEmbedUrl();
    }
}
