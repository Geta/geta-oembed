// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Models;
using System.Text.Json;

namespace Geta.OEmbed.Providers
{
    public class HttpClientManifestLoader : IProviderManifestLoader
    {
        private readonly OEmbedConfiguration _configuration;
        private readonly HttpClient _httpClient;
        
        public HttpClientManifestLoader(OEmbedConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public virtual async Task<IEnumerable<IOEmbedProvider>> GetEmbedProvidersAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(_configuration.ProviderManifestUrl, cancellationToken);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            return await JsonSerializer.DeserializeAsync<OEmbedProvider[]>(content, options: null, cancellationToken) ?? Enumerable.Empty<OEmbedProvider>();
        }
    }
}
