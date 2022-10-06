// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

namespace Geta.OEmbed.Client.Providers
{
    public interface IProviderUrlBuilder
    {
        bool CanBuild(IOEmbedProvider oEmbedProvider);

        string Build(string url, IOEmbedProvider oEmbedProvider, OEmbedOptions embedOptions);
    }
}
