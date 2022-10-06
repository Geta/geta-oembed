// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Client.Models;

namespace Geta.OEmbed.Client.Providers
{
    public interface IProviderResponseFormatter
    {
        bool CanFormat(IOEmbedProvider oEmbedProvider, OEmbedResponse oEmbedResponse);

        OEmbedResponse FormatResponse(OEmbedResponse oEmbedResponse, OEmbedOptions options);
    }
}
