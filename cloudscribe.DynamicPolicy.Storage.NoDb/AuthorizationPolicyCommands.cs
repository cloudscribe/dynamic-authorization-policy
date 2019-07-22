// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:					2018-01-01
// Last Modified:			2018-01-02
// 

using cloudscribe.DynamicPolicy.Models;
using NoDb;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Storage.NoDb
{
    public class AuthorizationPolicyCommands : IAuthorizationPolicyCommands
    {
        public AuthorizationPolicyCommands(
            IBasicCommands<AuthorizationPolicyInfo> policyCommands,
            IBasicQueries<AuthorizationPolicyInfo> policyQueries,
            PolicyCache cache
            )
        {
            _policyCommands = policyCommands;
            _policyQueries = policyQueries;
            _cache = cache;
        }

        private IBasicCommands<AuthorizationPolicyInfo> _policyCommands;
        private IBasicQueries<AuthorizationPolicyInfo> _policyQueries;
        private PolicyCache _cache;

        public async Task Create(
            AuthorizationPolicyInfo policy,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (policy == null) { throw new ArgumentException("policy cannot be null"); }

            await _policyCommands.CreateAsync(
                policy.TenantId,
                policy.Id.ToString(),
                policy,
                cancellationToken).ConfigureAwait(false);

            _cache.ClearListCache(policy.TenantId);
        }

        public async Task Update(
            AuthorizationPolicyInfo policy,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (policy == null) { throw new ArgumentException("policy cannot be null"); }

            await _policyCommands.UpdateAsync(
               policy.TenantId,
               policy.Id.ToString(),
               policy,
               cancellationToken).ConfigureAwait(false);

            _cache.ClearListCache(policy.TenantId);
        }

        public async Task Delete(
            string tenantId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _policyCommands.DeleteAsync(tenantId, id.ToString(), cancellationToken).ConfigureAwait(false);

            _cache.ClearListCache(tenantId);
        }

        public async Task DeleteByTenant(
            string tenantId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var all = await _policyQueries.GetAllAsync(tenantId);
            foreach (var p in all)
            {
                await Delete(tenantId, p.Id).ConfigureAwait(false);
            }

            _cache.ClearListCache(tenantId);
        }



    }
}
