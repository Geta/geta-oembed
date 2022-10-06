// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Framework.Cache;
using Geta.OEmbed.Client;

namespace Geta.OEmbed.Optimizely.Caching
{
    public class CachedOEmbedProviderRepository : IOEmbedProviderRepository
    {
        private readonly IOEmbedProviderRepository _oEmbedProviderRepository;
        private readonly ISynchronizedObjectInstanceCache _cache;
        private readonly OptimizelyOEmbedProvider _optimizelyOEmbedProvider;

        public CachedOEmbedProviderRepository(
            IOEmbedProviderRepository oEmbedProviderRepository,
            ISynchronizedObjectInstanceCache cache,
            OptimizelyOEmbedProvider optimizelyOEmbedProvider
        )
        {
            _oEmbedProviderRepository = oEmbedProviderRepository;
            _cache = cache;
            _optimizelyOEmbedProvider = optimizelyOEmbedProvider;
        }

        public virtual Task<IEnumerable<IOEmbedProvider>> ListAsync()
        {
            return ListAsync(CancellationToken.None);
        }

        public virtual async Task<IEnumerable<IOEmbedProvider>> ListAsync(CancellationToken cancellationToken)
        {
            var cacheKey = GetCacheKey(nameof(ListAsync));

            if (_cache.Get(cacheKey) is IEnumerable<IOEmbedProvider> cached)
            {
                return cached;
            }
            
            var providers = (await _oEmbedProviderRepository.ListAsync(cancellationToken)).ToList();
            providers.Insert(0, _optimizelyOEmbedProvider);
            _cache.Insert(cacheKey, providers, new CacheEvictionPolicy(TimeSpan.FromHours(12), CacheTimeoutType.Absolute));
            return providers;
        }

        public virtual Task<IOEmbedProvider?> FindByUrlAsync(string url)
        {
            return FindByUrlAsync(url, CancellationToken.None);
        }

        public async Task<IOEmbedProvider?> FindByUrlAsync(string url, CancellationToken cancellationToken)
        {
            var providers = await ListAsync(cancellationToken);
            return _oEmbedProviderRepository.FindByUrl(providers, url);
        }

        public IOEmbedProvider? FindByUrl(IEnumerable<IOEmbedProvider> providers, string url)
        {
            return _oEmbedProviderRepository.FindByUrl(providers, url);
        }

        protected virtual string GetCacheKey(string methodName, params string[] args)
        {
            return $"{nameof(CachedOEmbedProviderRepository)}:{methodName}:{string.Join(":", args)}";
        }
    }
}