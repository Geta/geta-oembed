// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Geta.OEmbed.Extensions;
using Geta.OEmbed.Models;

namespace Geta.OEmbed
{
    public class OEmbedService : IOEmbedService
    {
        private static readonly Regex _srcFilter = new("src=\"([a-z-0-9-_?=/:.]{1,})\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly IOEmbedProviderRepository _oEmbedProviderRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public OEmbedService(IOEmbedProviderRepository oEmbedProviderRepository, IHttpClientFactory httpClientFactory)
        {
            _oEmbedProviderRepository = oEmbedProviderRepository;
            _httpClientFactory = httpClientFactory;
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

            var parameters = string.Join("&", CreateParameters(options).Select(o => $"{o.Key}={o.Value}"));
            var oEmbedUrl = $"{provider.GetOEmbedUrl()}?url={HttpUtility.UrlEncode(url)}&{parameters}";

            var request = new HttpRequestMessage(HttpMethod.Get, oEmbedUrl)
            {
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") },
                }
            };

            using var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(request, cancellationToken);
            
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync(cancellationToken);
            var oEmbedEntry = await JsonSerializer.DeserializeAsync<OEmbedResponse>(content, cancellationToken: cancellationToken);
            if (oEmbedEntry is not null)
                ParseVideoUrl(oEmbedEntry, options);

            return oEmbedEntry;
        }

        protected virtual void ParseVideoUrl(OEmbedResponse oEmbedResponse, OEmbedOptions options)
        {
            if (oEmbedResponse.Type != "video")
            {
                return;
            }

            var match = _srcFilter.Match(oEmbedResponse.Html);
            if (!match.Success)
            {
                return;
            }

            var originalUrl = match.Groups[1].Value;
            var parameters = CreateReplacementParameters(options);
            var urlWithOptions = originalUrl.AddParameters(parameters);

            oEmbedResponse.Html = oEmbedResponse.Html.Replace(originalUrl, urlWithOptions);
        }

        protected virtual IDictionary<string, string> CreateReplacementParameters(OEmbedOptions options)
        {
            return new Dictionary<string, string>
            {
                { nameof(OEmbedOptions.Autoplay).ToLower(), options.Autoplay ? "1" : "0" },
                { nameof(OEmbedOptions.Controls).ToLower(), options.Controls ? "1" : "0" },
                { nameof(OEmbedOptions.Loop).ToLower(), options.Loop ? "1" : "0" },
                { nameof(OEmbedOptions.Muted).ToLower(), options.Muted ? "1" : "0" },
                { "mute", options.Muted ? "1" : "0" },
            };
        }

        protected virtual IDictionary<string, string> CreateParameters(OEmbedOptions options)
        {
            var parameters = new Dictionary<string, string>
            {
                { nameof(OEmbedOptions.Autoplay).ToLower(), options.Autoplay.ToString().ToLower() },
                { nameof(OEmbedOptions.Controls).ToLower(), options.Controls.ToString().ToLower() },
                { nameof(OEmbedOptions.Loop).ToLower(), options.Loop.ToString().ToLower() },
                { nameof(OEmbedOptions.Muted).ToLower(), options.Muted.ToString().ToLower() },
            };

            if (options.Width.HasValue)
            {
                parameters.Add(nameof(OEmbedOptions.Width).ToLower(), options.Width.Value.ToString());
            }

            if (options.Height.HasValue)
            {
                parameters.Add(nameof(OEmbedOptions.Height).ToLower(), options.Height.Value.ToString());
            }

            if (options.MaxWidth.HasValue)
            {
                parameters.Add(nameof(OEmbedOptions.MaxWidth).ToLower(), options.MaxWidth.Value.ToString());
            }

            if (options.MaxHeight.HasValue)
            {
                parameters.Add(nameof(OEmbedOptions.MaxHeight).ToLower(), options.MaxHeight.Value.ToString());
            }

            return parameters;
        }
    }
}