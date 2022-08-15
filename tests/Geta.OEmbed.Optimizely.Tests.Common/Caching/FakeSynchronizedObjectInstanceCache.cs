// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer.Framework.Cache;

namespace Geta.OEmbed.Tests.Common.Caching
{
    public class FakeSynchronizedObjectInstanceCache : ISynchronizedObjectInstanceCache
    {
        private readonly IObjectInstanceCache _objectInstanceCache;

        public FakeSynchronizedObjectInstanceCache(IObjectInstanceCache objectInstanceCache)
        {
            _objectInstanceCache = objectInstanceCache;
        }

        public FailureRecoveryAction SynchronizationFailedStrategy { get; set; }

        public IObjectInstanceCache ObjectInstanceCache => _objectInstanceCache;

        [Obsolete]
        public void Clear()
        {
            _objectInstanceCache.Clear();
        }

        public object? Get(string key)
        {
            return _objectInstanceCache.Get(key);
        }

        public void Insert(string key, object value, CacheEvictionPolicy evictionPolicy)
        {
            _objectInstanceCache.Insert(key, value, evictionPolicy);
        }

        public void Remove(string key)
        {
            _objectInstanceCache.Remove(key);
        }

        public void RemoveLocal(string key)
        {
            _objectInstanceCache.Remove(key);
        }

        public void RemoveRemote(string key)
        {
            _objectInstanceCache.Remove(key);
        }
    }
}
