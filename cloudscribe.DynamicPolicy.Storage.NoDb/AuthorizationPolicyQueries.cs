// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:					2018-01-01
// Last Modified:			2019-03-25
// 

using cloudscribe.DynamicPolicy.Models;
using cloudscribe.Pagination.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Storage.NoDb
{
    public class AuthorizationPolicyQueries : IAuthorizationPolicyQueries
    {
        public AuthorizationPolicyQueries(
            PolicyCache cache,
            IBasicQueries<AuthorizationPolicyInfo> policyQueries
            )
        {
            _policyQueries = policyQueries;
            _cache = cache;
        }

        private IBasicQueries<AuthorizationPolicyInfo> _policyQueries;
        private PolicyCache _cache;

        public async Task<List<AuthorizationPolicyInfo>> GetAll(
            string tenantId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var list = _cache.GetAll(tenantId);
            if (list != null) return list;

            var l = await _policyQueries.GetAllAsync(tenantId, cancellationToken).ConfigureAwait(false);
            list = l.ToList();

            if (list.Count > 0)
            {
                _cache.AddToCache(list, tenantId);
            }


            return list;

        }

        public async Task<AuthorizationPolicyInfo> Fetch(
            string tenantId,
            Guid policyId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _policyQueries.FetchAsync(
                tenantId,
                policyId.ToString(),
                cancellationToken).ConfigureAwait(false);

        }

        public async Task<AuthorizationPolicyInfo> Fetch(
            string tenantId,
            string policyName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var list = await GetAll(tenantId, cancellationToken).ConfigureAwait(false);
            return list.FirstOrDefault(p => p.Name == policyName);

        }

        public async Task<PagedResult<AuthorizationPolicyInfo>> GetPage(
            string tenantId,
            string searchQuery,
            int pageNumber,
            int pageSize,
            string filterRoles = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var all = await GetAll(tenantId, cancellationToken).ConfigureAwait(false);

            if (searchQuery == null) { searchQuery = string.Empty; }

            // Parse role filters
            List<string> roleFilters = null;
            if (!string.IsNullOrWhiteSpace(filterRoles))
            {
                roleFilters = filterRoles.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(r => r.Trim())
                                          .ToList();
            }

            // Apply text search filter
            var query = all.AsQueryable()
               .Where(x => searchQuery == string.Empty || x.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase));

            // Apply role filter (OR logic - policy must have ANY of the selected roles)
            if (roleFilters != null && roleFilters.Count > 0)
            {
                query = query.Where(x => x.AllowedRoles.Any(r => roleFilters.Contains(r)));
            }

            var result = new PagedResult<AuthorizationPolicyInfo>
            {
                TotalItems = query.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            query = query
                .OrderBy(x => x.Name)
                .Select(p => p)
                .Skip(offset)
                .Take(pageSize)
                ;

            result.Data = query.ToList<AuthorizationPolicyInfo>();

            return result;

        }

    }
}
