// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.OEmbed.Providers;
using System.Text.RegularExpressions;

namespace Geta.OEmbed
{
    public class OEmbedProviderRepository : IOEmbedProviderRepository
    {
        private readonly IProviderManifestLoader _providerManifestLoader;

        public OEmbedProviderRepository(IProviderManifestLoader providerManifestLoader)
        {
            _providerManifestLoader = providerManifestLoader;
        }

        public virtual async Task<IEnumerable<IOEmbedProvider>> ListAsync()
        {
            return await ListAsync(CancellationToken.None);
        }

        public virtual Task<IEnumerable<IOEmbedProvider>> ListAsync(CancellationToken cancellationToken)
        {
            return _providerManifestLoader.GetEmbedProvidersAsync(cancellationToken);
        }

        public virtual Task<IOEmbedProvider?> FindByUrlAsync(string url)
        {
            return FindByUrlAsync(url, CancellationToken.None);
        }

        public virtual async Task<IOEmbedProvider?> FindByUrlAsync(string url, CancellationToken cancellationToken)
        {
            var list = await ListAsync(cancellationToken);
            return FindByUrl(list, url);
        }

        public virtual IOEmbedProvider? FindByUrl(IEnumerable<IOEmbedProvider> providers, string url)
        {
            return providers.FirstOrDefault(p => IsMatch(p, url));
        }

        protected virtual bool IsMatch(IOEmbedProvider provider, string url)
        {
            if (provider?.Endpoints is null)
                return false;

            var schemesRegEx = provider
                .Endpoints
                .Where(endpoint => endpoint.Schemes is not null)
                .SelectMany(endpoint => endpoint.Schemes)
                .Where(scheme => !string.IsNullOrEmpty(scheme))
                .Select(scheme => new Regex(@$"^{ToRegExString(scheme)}$"))
                .ToList();
    
            var isMatch = schemesRegEx.Any(x => x.IsMatch(url));
            return isMatch;
        }

        protected virtual string ToRegExString(string scheme)
        {
            scheme = scheme
                .Replace("/", @"\/")
                .Replace(".", @"\.")
                .Replace("*", "(.{1,})");
            
            return scheme;
        }
    }
}