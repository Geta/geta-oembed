﻿// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Client;

namespace Geta.OEmbed.Client.Providers
{
    public interface IProviderManifestLoader
    {
        Task<IEnumerable<IOEmbedProvider>> GetEmbedProvidersAsync(CancellationToken cancellationToken);
    }
}
