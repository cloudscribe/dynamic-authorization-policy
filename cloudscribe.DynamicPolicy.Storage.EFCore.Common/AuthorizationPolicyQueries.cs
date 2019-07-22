using cloudscribe.DynamicPolicy.Models;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common
{
    public class AuthorizationPolicyQueries : IAuthorizationPolicyQueries
    {
        public AuthorizationPolicyQueries(
            IDynamicPolicyDbContextFactory contextFactory,
            PolicyCache cache
            )
        {
            _contextFactory = contextFactory;
            _cache = cache;
        }

        private readonly IDynamicPolicyDbContextFactory _contextFactory;
        private PolicyCache _cache;

        private async Task<List<AuthorizationPolicyInfo>> GetAll(
            string tenantId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {

            var list = _cache.GetAll(tenantId);
            if (list != null) return list;

            using (var db = _contextFactory.CreateContext())
            {
                var all = db.Policies
                   .AsNoTracking()
                   .Include(p => p.AllowedRoles)
                   .Include(p => p.AuthenticationSchemes)
                   .Include(p => p.RequiredClaims)
                   .ThenInclude(x => x.AllowedValues)
                   .Where(p => p.TenantId == tenantId)
                   ;

                list = await all.Select(x => x.ToAuthorizationPolicyInfo()).ToListAsync().ConfigureAwait(false);

                if (list.Count > 0)
                {
                    _cache.AddToCache(list, tenantId);
                }

                return list;
            }

               

        }

        public async Task<AuthorizationPolicyInfo> Fetch(
            string tenantId,
            Guid policyId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var all = await GetAll(tenantId).ConfigureAwait(false);
            return all.FirstOrDefault(x => x.Id == policyId);

        }

        public async Task<AuthorizationPolicyInfo> Fetch(
            string tenantId,
            string policyName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var all = await GetAll(tenantId).ConfigureAwait(false);
            return all.FirstOrDefault(x => x.Name == policyName);

        }

        public async Task<PagedResult<AuthorizationPolicyInfo>> GetPage(
            string tenantId,
            string searchQuery,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var all = await GetAll(tenantId, cancellationToken).ConfigureAwait(false);

            if(searchQuery == null) { searchQuery = string.Empty; }

            var query = all.AsQueryable()
               .Where(x => searchQuery == string.Empty || x.Name.StartsWith(searchQuery, StringComparison.InvariantCultureIgnoreCase));

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
