// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Framework.Cache;
using Geta.OEmbed.Models;

namespace Geta.OEmbed.Optimizely.Caching
{
    public class CachedOEmbedService : IOEmbedService
    {
        private readonly IOEmbedService _oEmbedService;
        private readonly ISynchronizedObjectInstanceCache _cache;
        private readonly Lazy<CacheEvictionPolicy> _cacheEvictionPolicy;

        public CachedOEmbedService(IOEmbedService oEmbedService, ISynchronizedObjectInstanceCache cache)
        {
            _oEmbedService = oEmbedService;
            _cache = cache;
            _cacheEvictionPolicy = new Lazy<CacheEvictionPolicy>(() => CreateCacheEvictionPolicy());
        }
        
        public virtual Task<OEmbedResponse?> GetAsync(string url)
        {
            return GetAsync(url, new OEmbedOptions(), CancellationToken.None);
        }

        public virtual Task<OEmbedResponse?> GetAsync(string url, CancellationToken cancellationToken)
        {
            return GetAsync(url, new OEmbedOptions(), cancellationToken);
        }

        public virtual Task<OEmbedResponse?> GetAsync(string url, OEmbedOptions options)
        {
            return GetAsync(url, options, CancellationToken.None);
        }

        public virtual async Task<OEmbedResponse?> GetAsync(string url, OEmbedOptions options, CancellationToken cancellationToken)
        {
            var cacheKey = GetCacheKey(nameof(GetAsync), url, options);

            if (_cache.Get(cacheKey) is OEmbedResponse cachedOEmbedEntry)
            {
                return cachedOEmbedEntry;
            }
            
            var oEmbedEntry = await _oEmbedService.GetAsync(url, options, cancellationToken);
            _cache.Insert(cacheKey, oEmbedEntry, _cacheEvictionPolicy.Value);

            return oEmbedEntry;
        }

        protected virtual string GetCacheKey(string methodName, params object[] args)
        {
            return $"{nameof(CachedOEmbedService)}:{methodName}:{string.Join(":", args)}";
        }

        protected virtual CacheEvictionPolicy CreateCacheEvictionPolicy()
        {
            return new CacheEvictionPolicy(TimeSpan.FromMinutes(15), CacheTimeoutType.Absolute);
        }
    }
}