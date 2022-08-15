// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Models;
using System.Text.Json;

namespace Geta.OEmbed.Providers
{
    public class HttpClientManifestLoader : IProviderManifestLoader
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientManifestLoader(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public virtual async Task<IEnumerable<IOEmbedProvider>> GetEmbedProvidersAsync(CancellationToken cancellationToken)
        {
            using var client = _clientFactory.CreateClient();

            var response = await client.GetAsync("https://oembed.com/providers.json", cancellationToken);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            return await JsonSerializer.DeserializeAsync<OEmbedProvider[]>(content, options: null, cancellationToken) ?? Enumerable.Empty<OEmbedProvider>();
        }
    }
}
