// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.OEmbed.Providers
{
    public interface IProviderManifestLoader
    {
        Task<IEnumerable<IOEmbedProvider>> GetEmbedProvidersAsync(CancellationToken cancellationToken);
    }
}
