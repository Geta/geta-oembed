// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Web;
using Geta.OEmbed.Models;
using Microsoft.Extensions.Options;

namespace Geta.OEmbed.Optimizely
{
    public class OptimizelyOEmbedProvider : IOEmbedProvider
    {
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly IOptions<UIOptions> _uiOptions;
        private readonly Lazy<List<OEmbedEndpoint>> _endpoints;
        private readonly Lazy<string> _providerUrl;        

        public OptimizelyOEmbedProvider(ISiteDefinitionRepository siteDefinitionRepository, IOptions<UIOptions> uiOptions)
        {
            _siteDefinitionRepository = siteDefinitionRepository;
            _uiOptions = uiOptions;
            _endpoints = new Lazy<List<OEmbedEndpoint>>(() => GetEndpoints());
            _providerUrl = new Lazy<string>(() => GetProviderUrl());

            ProviderName = "Optimizely";
        }

        public virtual string ProviderName { get; }

        public virtual string ProviderUrl => _providerUrl.Value;

        public virtual List<OEmbedEndpoint> Endpoints => _endpoints.Value;

        public virtual string GetOEmbedUrl()
        {
            return FormatEndpointUrl(Endpoints.First().Url);
        }

        protected virtual List<OEmbedEndpoint> GetEndpoints()
        {
            var endpoints = new List<OEmbedEndpoint>();

            foreach (var definition in _siteDefinitionRepository.List())
            {
                var editHost = definition.Hosts.FirstOrDefault(x => x.Type == HostDefinitionType.Edit);
                if (editHost is null)
                {
                    continue;
                }

                var endpoint = GetEndpoint(definition);
                endpoints.Add(endpoint);
            }

            return endpoints;
        }

        protected virtual OEmbedEndpoint GetEndpoint(SiteDefinition definition)
        {
            return new OEmbedEndpoint
            {
                Url = GetEndpointUrl(definition),
                Schemes = GetSchemes(definition),
                Discovery = false
            };
        }

        protected virtual string GetEndpointUrl(SiteDefinition definition)
        {
            var siteUrl = definition.SiteUrl;
            var editUrl = _uiOptions.Value.EditUrl.ToString();

            return $"{siteUrl}{editUrl.TrimStart('~').TrimStart('/')}oembed";
        }

        protected virtual List<string> GetSchemes(SiteDefinition definition)
        {
            var schemes = new List<string>
            {
                $"{definition.SiteUrl}globalassets/*.mp4|webm",
                $"{definition.SiteUrl}globalassets/*.jpg|gif|png|webp",
                $"{definition.SiteUrl}link/*.aspx",
                "/globalassets/*.mp4|webm",
                "/globalassets/*.jpg|gif|png|webp",
                "/link/*.aspx"
            };

            if (!definition.SiteAssetsRoot.CompareToIgnoreWorkID(definition.GlobalAssetsRoot))
            {
                schemes.Add($"{definition.SiteUrl}siteassets/*.mp4|webm");
                schemes.Add($"{definition.SiteUrl}siteassets/*.jpg|gif|png|webp");
                schemes.Add("/siteassets/*.mp4|webm");
                schemes.Add("/siteassets/*.jpg|gif|png|webp");
            }

            return schemes;
        }        

        protected virtual string GetProviderUrl()
        {
            return Endpoints.FirstOrDefault()?.Url ?? string.Empty;
        }

        protected virtual string FormatEndpointUrl(string url)
        {
            return url.Replace("{format}", "json");
        }
    }
}