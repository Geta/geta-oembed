// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Web;
using Geta.OEmbed.Client;
using Geta.OEmbed.Client.Models;
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

        protected virtual List<OEmbedEndpoint> GetEndpoints()
        {
            var endpoints = new List<OEmbedEndpoint>();

            foreach (var definition in _siteDefinitionRepository.List())
            {
                var editHost = GetEditHost(definition);

                if (editHost == null)
                {
                    continue;
                }

                var endpoint = GetEndpoint(definition, editHost);
                endpoints.Add(endpoint);
            }

            return endpoints;
        }

        protected virtual HostDefinition? GetEditHost(SiteDefinition definition)
        {
            return definition.Hosts.FirstOrDefault(x => x.Type == HostDefinitionType.Edit) ??
                   definition.Hosts.FirstOrDefault(x => x.Type == HostDefinitionType.Primary);
        }

        protected virtual OEmbedEndpoint GetEndpoint(SiteDefinition definition, HostDefinition editHost)
        {
            return new OEmbedEndpoint
            {
                Url = GetEndpointUrl(editHost),
                Schemes = GetSchemes(definition, editHost),
                Discovery = false
            };
        }

        protected virtual string GetEndpointUrl(HostDefinition editHost)
        {
            var siteUrl = editHost.Url;
            var editUrl = _uiOptions.Value.EditUrl.ToString();

            return $"{siteUrl}{editUrl.TrimStart('~').TrimStart('/')}oembed";
        }

        protected virtual List<string> GetSchemes(SiteDefinition definition, HostDefinition editHost)
        {
            var schemes = new List<string>
            {
                $"{editHost.Url}globalassets/*.mp4|webm",
                $"{editHost.Url}globalassets/*.jpg|gif|png|webp",
                $"{editHost.Url}link/*.aspx",
                "/globalassets/*.mp4|webm",
                "/globalassets/*.jpg|gif|png|webp",
                "/link/*.aspx"
            };

            if (!definition.SiteAssetsRoot.CompareToIgnoreWorkID(definition.GlobalAssetsRoot))
            {
                schemes.Add($"{editHost.Url}siteassets/*.mp4|webm");
                schemes.Add($"{editHost.Url}siteassets/*.jpg|gif|png|webp");
                schemes.Add("/siteassets/*.mp4|webm");
                schemes.Add("/siteassets/*.jpg|gif|png|webp");
            }

            return schemes;
        }

        protected virtual string GetProviderUrl()
        {
            return Endpoints.FirstOrDefault()?.Url ?? string.Empty;
        }
    }
}
