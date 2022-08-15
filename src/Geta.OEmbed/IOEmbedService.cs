// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Models;

namespace Geta.OEmbed
{
    public interface IOEmbedService
    {
        Task<OEmbedResponse?> GetAsync(string url);
        Task<OEmbedResponse?> GetAsync(string url, CancellationToken cancellationToken);
        Task<OEmbedResponse?> GetAsync(string url, OEmbedOptions options);
        Task<OEmbedResponse?> GetAsync(string url, OEmbedOptions options, CancellationToken cancellationToken);
    }
}