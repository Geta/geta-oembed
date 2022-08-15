// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Framework.Cache;

namespace Geta.OEmbed.Tests.Common.Caching
{
    public class MemoryInstanceCache : IObjectInstanceCache
    {
        private readonly IDictionary<string, (object, CacheEvictionPolicy)> _cache;

        public MemoryInstanceCache()
        {
            _cache = new Dictionary<string, (object, CacheEvictionPolicy)>();
        }

        public virtual IDictionary<string, (object, CacheEvictionPolicy)> Cache => _cache;

        public virtual void Clear()
        {
            _cache.Clear();
        }

        public virtual object? Get(string key)
        {
            if (_cache.TryGetValue(key, out var value))
                return value.Item1;

            return null;
        }

        public virtual void Insert(string key, object value, CacheEvictionPolicy evictionPolicy)
        {
            _cache.TryAdd(key, (value, evictionPolicy));
        }

        public virtual void Remove(string key)
        {
            if (_cache.ContainsKey(key))
                _cache.Remove(key);
        }
    }
}
