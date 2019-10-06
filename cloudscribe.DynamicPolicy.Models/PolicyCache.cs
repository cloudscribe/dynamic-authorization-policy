using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace cloudscribe.DynamicPolicy.Models
{
    public class PolicyCache
    {
        public PolicyCache(
            IMemoryCache cache,
            IOptions<PolicyCacheOptions> optionsAccessor
            )
        {
            if (cache == null) { throw new ArgumentNullException(nameof(cache)); }
            _cache = cache;
            _options = optionsAccessor.Value;
        }

        private IMemoryCache _cache;
        private PolicyCacheOptions _options;

        public List<AuthorizationPolicyInfo> GetAll(
            string tenantId
            )
        {
            var cacheKey = GetListCacheKey(tenantId);
            List<AuthorizationPolicyInfo> result = (List<AuthorizationPolicyInfo>)_cache.Get(cacheKey);

            return result;

        }

        public void AddToCache(List<AuthorizationPolicyInfo> policyList, string tenantId)
        {
            var cacheKey = GetListCacheKey(tenantId);
            _cache.Set(
                cacheKey,
                policyList,
                new MemoryCacheEntryOptions()
                { 
                    Size = _options.CacheItemSize
                }
                 .SetSlidingExpiration(TimeSpan.FromSeconds(_options.CacheDurationInSeconds))
                 );
        }

        public string GetListCacheKey(string tenantId)
        {
            return tenantId + "-auth-policylist";
        }

        public void ClearListCache(string tenantId)
        {
            var cacheKey = GetListCacheKey(tenantId);
            _cache.Remove(cacheKey);


        }


    }
}
