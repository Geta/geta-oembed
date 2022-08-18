// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Castle.Components.DictionaryAdapter;
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

        public virtual FailureRecoveryAction SynchronizationFailedStrategy { get; set; }

        public virtual IObjectInstanceCache ObjectInstanceCache => _objectInstanceCache;

        [Obsolete("Method is no longer supported. HttpRuntimeCache can be clearad by iterating over all keys and remove each item")]
        public virtual void Clear()
        {
            _objectInstanceCache.Clear();
        }

        public virtual object? Get(string key)
        {
            return _objectInstanceCache.Get(key);
        }

        public virtual void Insert(string key, object value, CacheEvictionPolicy evictionPolicy)
        {
            _objectInstanceCache.Insert(key, value, evictionPolicy);
        }

        public virtual void Remove(string key)
        {
            _objectInstanceCache.Remove(key);
        }

        public virtual void RemoveLocal(string key)
        {
            _objectInstanceCache.Remove(key);
        }

        public virtual void RemoveRemote(string key)
        {
            _objectInstanceCache.Remove(key);
        }
    }
}
