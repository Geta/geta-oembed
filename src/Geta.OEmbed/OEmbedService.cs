// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Net.Http.Headers;
using System.Text.Json;
using Geta.OEmbed.Models;
using Geta.OEmbed.Providers;

namespace Geta.OEmbed
{
    public class OEmbedService : IOEmbedService
    {
        private readonly IOEmbedProviderRepository _oEmbedProviderRepository;
        private readonly IEnumerable<IProviderUrlBuilder> _providerUrlBuilders;
        private readonly IEnumerable<IProviderResponseFormatter> _embedFormatters;
        private readonly HttpClient _httpClient;

        public OEmbedService(IOEmbedProviderRepository oEmbedProviderRepository, IEnumerable<IProviderUrlBuilder> providerUrlBuilders, IEnumerable<IProviderResponseFormatter> responseFormatters, HttpClient httpClient)
        {
            _oEmbedProviderRepository = oEmbedProviderRepository;
            _providerUrlBuilders = providerUrlBuilders;
            _embedFormatters = responseFormatters;
            _httpClient = httpClient;
        }

        public virtual async Task<OEmbedResponse?> GetAsync(string url)
        {
            return await GetAsync(url, CancellationToken.None);
        }

        public virtual Task<OEmbedResponse?> GetAsync(string url, CancellationToken cancellationToken)
        {
            return GetAsync(url, new OEmbedOptions(), CancellationToken.None);
        }

        public virtual Task<OEmbedResponse?> GetAsync(string url, OEmbedOptions options)
        {
            return GetAsync(url, options, CancellationToken.None);
        }

        public virtual async Task<OEmbedResponse?> GetAsync(string url, OEmbedOptions options, CancellationToken cancellationToken)
        {
            var provider = await _oEmbedProviderRepository.FindByUrlAsync(url, cancellationToken);
            if (provider is null)
            {
                return null;
            }

            var providerUrlBuilder = GetProviderUrlBuilder(provider);
            var oEmbedUrl = providerUrlBuilder.Build(url, provider, options);

            var request = new HttpRequestMessage(HttpMethod.Get, oEmbedUrl)
            {
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") },
                }
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);
            
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync(cancellationToken);
            var oEmbedEntry = await JsonSerializer.DeserializeAsync<OEmbedResponse>(content, cancellationToken: cancellationToken);

            if (oEmbedEntry is not null)
            {
                oEmbedEntry = FormatResponse(provider, oEmbedEntry, options);
            }

            return oEmbedEntry;
        }

        protected virtual OEmbedResponse FormatResponse(IOEmbedProvider embedProvider, OEmbedResponse oEmbedResponse, OEmbedOptions options)
        {
            var formatter = GetProviderResponseFormatter(embedProvider, oEmbedResponse);
            if (formatter is null)
            {
                return oEmbedResponse;
            }

            return formatter.FormatResponse(oEmbedResponse, options);
        }

        protected virtual IProviderUrlBuilder GetProviderUrlBuilder(IOEmbedProvider embedProvider)
        {
            return _providerUrlBuilders.First(x => x.CanBuild(embedProvider));
        }

        protected virtual IProviderResponseFormatter? GetProviderResponseFormatter(IOEmbedProvider embedProvider, OEmbedResponse oEmbedResponse)
        {
            return _embedFormatters.FirstOrDefault(x => x.CanFormat(embedProvider, oEmbedResponse));
        }
    }
}