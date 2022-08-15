// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.OEmbed
{
    public interface IOEmbedProviderRepository
    {
        Task<IEnumerable<IOEmbedProvider>> ListAsync();
        Task<IEnumerable<IOEmbedProvider>> ListAsync(CancellationToken cancellationToken);
        Task<IOEmbedProvider?> FindByUrlAsync(string url);
        Task<IOEmbedProvider?> FindByUrlAsync(string url, CancellationToken cancellationToken);
        IOEmbedProvider? FindByUrl(IEnumerable<IOEmbedProvider> providers, string url);
    }
}