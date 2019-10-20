using cloudscribe.DynamicPolicy.Models;
using cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                SyncRoles(db, policy.AllowedRoles, entity);
                SyncSchemes(db, policy.AuthenticationSchemes, entity);
                SyncClaimRequirements(db, policy.RequiredClaims, entity);

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
                SyncRoles(db, policy.AllowedRoles, entity);
                SyncSchemes(db, policy.AuthenticationSchemes, entity);
                SyncClaimRequirements(db, policy.RequiredClaims, entity);



                //db.Policies.(entity).State = EntityState.Modified;

                bool tracking = db.ChangeTracker.Entries<AuthorizationPolicyInfo>().Any(x => x.Entity.Id == entity.Id);
                if (!tracking)
                {
                    db.Policies.Update(entity);
                }

               // var saved = false;
                //while (!saved)
                //{
                    try
                    {
                        int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);

                        //saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        //foreach (var entry in ex.Entries)
                        //{
                        //    if (entry.Entity is AuthorizationPolicyEntity)
                        //    {
                        //        var proposedValues = entry.CurrentValues;
                        //        var databaseValues = entry.GetDatabaseValues();

                        //        foreach (var property in proposedValues.Properties)
                        //        {
                        //            var proposedValue = proposedValues[property];
                        //            var databaseValue = databaseValues[property];

                        //            // TODO: decide which value should be written to database
                        //            // proposedValues[property] = <value to be saved>;
                        //        }

                        //        // Refresh original values to bypass next concurrency check
                        //        entry.OriginalValues.SetValues(databaseValues);
                        //    }
                        //    else
                        //    {
                        //        throw new NotSupportedException(
                        //            "Don't know how to handle concurrency conflicts for "
                        //            + entry.Metadata.Name);
                        //    }
                        //}
                    }
                //}

                

                

                _cache.ClearListCache(policy.TenantId);
            }

            
        }

        private void SyncClaimRequirements(
            IDynamicPolicyDbContext db,
            List<ClaimRequirement> requiredClaims, 
            AuthorizationPolicyEntity entity)
        {
            if (requiredClaims.Count == 0)
            {
                entity.RequiredClaims.Clear();
            }
            else
            {
                if (entity.RequiredClaims.Count > 0)
                {
                    for (int i = 0; i < entity.RequiredClaims.Count; i++)
                    {
                        if (!requiredClaims.HasClaim(entity.RequiredClaims[i].ClaimName))
                        {
                            db.ClaimRequirements.Remove(entity.RequiredClaims[i]);
                            entity.RequiredClaims.RemoveAt(i);
                            
                        }
                    }
                }

                foreach (var c in requiredClaims)
                {
                    var found = entity.RequiredClaims.FindClaim(c.ClaimName);
                    if (found != null)
                    {
                        //c.SyncTo(found);
                        SyncClaimAllowedValues(db, c, found);
                    }
                    else
                    {
                        var newClaim = new ClaimRequirementEntity();
                        newClaim.ClaimName = c.ClaimName;
                        newClaim.Policy = entity;
                        // c.SyncTo(newClaim);
                        SyncClaimAllowedValues(db, c, newClaim);

                        entity.RequiredClaims.Add(newClaim);
                        db.ClaimRequirements.Add(newClaim);
                    }
                }


            }

        }

        private void SyncClaimAllowedValues(
            IDynamicPolicyDbContext db,
            ClaimRequirement claim, 
            ClaimRequirementEntity entity)
        {
            if (claim.AllowedValues.Count == 0)
            {
                entity.AllowedValues.Clear();
            }
            else
            {
                if (entity.AllowedValues.Count > 0)
                {
                    for (int i = 0; i < entity.AllowedValues.Count; i++)
                    {
                        if (!claim.AllowedValues.Contains(entity.AllowedValues[i].AllowedValue))
                        {
                            db.AllowedClaimValues.Remove(entity.AllowedValues[i]);
                            entity.AllowedValues.RemoveAt(i);
                            
                        }
                    }
                }

                foreach (var s in claim.AllowedValues)
                {
                    if (!entity.AllowedValues.HasRequiredValue(s))
                    {
                        var r = new AllowedClaimValueEntity();
                        r.AllowedValue = s;
                        r.ClaimRequirement = entity;
                        entity.AllowedValues.Add(r);
                        db.AllowedClaimValues.Add(r);
                    }
                }


            }

        }



        private static void SyncSchemes(
            IDynamicPolicyDbContext db,
            List<string> authSchemeStrings, 
            AuthorizationPolicyEntity entity)
        {
            if (authSchemeStrings.Count == 0)
            {
                entity.AuthenticationSchemes.Clear();
            }
            else
            {
                if (entity.AuthenticationSchemes.Count > 0)
                {
                    for (int i = 0; i < entity.AuthenticationSchemes.Count; i++)
                    {
                        if (!authSchemeStrings.Contains(entity.AuthenticationSchemes[i].AuthenticationScheme))
                        {
                            db.AuthenticationSchemes.Remove(entity.AuthenticationSchemes[i]);
                            entity.AuthenticationSchemes.RemoveAt(i);
                            
                        }
                    }
                }

                foreach (var s in authSchemeStrings)
                {
                    if (!entity.AuthenticationSchemes.HasScheme(s))
                    {
                        var r = new AuthenticationSchemeEntity();
                        r.AuthenticationScheme = s;
                        r.Policy = entity;
                        entity.AuthenticationSchemes.Add(r);
                        db.AuthenticationSchemes.Add(r);
                    }
                }

            }

        }

        private void SyncRoles(
            IDynamicPolicyDbContext db,
            List<string> allowedRoleStrings,
            //List<AllowedRoleEntity> allowedRoles,
            AuthorizationPolicyEntity entity)
        {
            if (allowedRoleStrings.Count == 0)
            {
                entity.AllowedRoles.Clear();
            }
            else
            {
                if (entity.AllowedRoles.Count > 0)
                {
                    for (int i = 0; i < entity.AllowedRoles.Count; i++)
                    {
                        if (!allowedRoleStrings.Contains(entity.AllowedRoles[i].AllowedRole))
                        {
                            db.AllowedRoles.Remove(entity.AllowedRoles[i]);
                            entity.AllowedRoles.RemoveAt(i);
                            
                        }
                    }
                }

                foreach (var s in allowedRoleStrings)
                {
                    if (!entity.AllowedRoles.HasRole(s))
                    {
                        var r = new AllowedRoleEntity();
                        r.AllowedRole = s;
                        r.Policy = entity;
                        entity.AllowedRoles.Add(r);
                        db.AllowedRoles.Add(r);
                    }
                }

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
