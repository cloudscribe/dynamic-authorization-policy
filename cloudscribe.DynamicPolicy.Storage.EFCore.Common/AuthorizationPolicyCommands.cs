using cloudscribe.DynamicPolicy.Models;
using cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common
{
    public class AuthorizationPolicyCommands : IAuthorizationPolicyCommands
    {
        public AuthorizationPolicyCommands(
            IDynamicPolicyDbContextFactory contextFactory,
            PolicyCache cache
            )
        {
            _contextFactory = contextFactory;
            _cache = cache;
        }

        private readonly IDynamicPolicyDbContextFactory _contextFactory;
        private PolicyCache _cache;

        public async Task Create(
            AuthorizationPolicyInfo policy,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (policy == null) { throw new ArgumentException("policy cannot be null"); }

            using (var db = _contextFactory.CreateContext())
            {
                var entity = new AuthorizationPolicyEntity();
                policy.CopyTo(entity);
                db.Policies.Add(entity);

                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                _cache.ClearListCache(policy.TenantId);
            }
            
        }

        public async Task Update(
            AuthorizationPolicyInfo policy,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (policy == null) { throw new ArgumentException("policy cannot be null"); }

            using (var db = _contextFactory.CreateContext())
            {
                var entity = await db.Policies
                .Include(p => p.AllowedRoles)
                .Include(p => p.AuthenticationSchemes)
                .Include(p => p.RequiredClaims)
                .ThenInclude(x => x.AllowedValues)
                .FirstOrDefaultAsync(p => p.Id == policy.Id, cancellationToken)
                .ConfigureAwait(false);

                policy.CopyTo(entity);

                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                _cache.ClearListCache(policy.TenantId);
            }

            
        }

        public async Task Delete(
            string tenantId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var db = _contextFactory.CreateContext())
            {
                var entity = await db.Policies
                .Include(p => p.AllowedRoles)
                .Include(p => p.AuthenticationSchemes)
                .Include(p => p.RequiredClaims)
                .ThenInclude(x => x.AllowedValues)
                .FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId, cancellationToken)
                .ConfigureAwait(false);

                if (entity != null)
                {
                    db.Policies.Remove(entity);
                    await db.SaveChangesAsync().ConfigureAwait(false);
                }

                _cache.ClearListCache(tenantId);
            }

           
        }

        public async Task DeleteByTenant(
            string tenantId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var db = _contextFactory.CreateContext())
            {
                var query = from x in db.Policies.Where(x => x.TenantId == tenantId)
                            select x;

                db.Policies.RemoveRange(query);
                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

           
        }

    }
}
