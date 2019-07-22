using cloudscribe.DynamicPolicy.Models;
using cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities;
using System.Collections.Generic;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common
{
    public static class ModelExtensions
    {

        public static AuthorizationPolicyInfo ToAuthorizationPolicyInfo(this AuthorizationPolicyEntity entity)
        {
            var policy = new AuthorizationPolicyInfo();
            policy.Id = entity.Id;
            policy.Name = entity.Name;
            policy.TenantId = entity.TenantId;
            policy.RequireAuthenticatedUser = entity.RequireAuthenticatedUser;
            policy.RequiredUserName = entity.RequiredUserName;
            policy.Notes = entity.Notes;
            foreach (var r in entity.AllowedRoles)
            {
                policy.AllowedRoles.Add(r.AllowedRole);
            }
            foreach (var s in entity.AuthenticationSchemes)
            {
                policy.AuthenticationSchemes.Add(s.AuthenticationScheme);
            }
            foreach (var c in entity.RequiredClaims)
            {
                var cr = new ClaimRequirement();
                cr.ClaimName = c.ClaimName;
                foreach (var r in c.AllowedValues)
                {
                    cr.AllowedValues.Add(r.AllowedValue);
                }
                policy.RequiredClaims.Add(cr);
            }

            return policy;

        }

        public static void CopyTo(this AuthorizationPolicyInfo policy, AuthorizationPolicyEntity entity)
        {
            entity.Id = policy.Id;
            entity.Name = policy.Name;
            entity.TenantId = policy.TenantId;
            entity.RequireAuthenticatedUser = policy.RequireAuthenticatedUser;
            entity.RequiredUserName = policy.RequiredUserName;
            entity.Notes = policy.Notes;
            policy.AllowedRoles.SyncTo(entity.AllowedRoles, entity);
            policy.AuthenticationSchemes.SyncTo(entity.AuthenticationSchemes, entity);
            policy.RequiredClaims.SyncTo(entity.RequiredClaims, entity);

        }

        public static void SyncTo(this List<ClaimRequirement> requiredClaims, List<ClaimRequirementEntity> claimEntities, AuthorizationPolicyEntity entity)
        {
            if (requiredClaims.Count == 0)
            {
                claimEntities.Clear();
            }
            else
            {
                if (claimEntities.Count > 0)
                {
                    for (int i = 0; i < claimEntities.Count; i++)
                    {
                        if (!requiredClaims.HasClaim(claimEntities[i].ClaimName))
                        {
                            claimEntities.RemoveAt(i);
                        }
                    }
                }

                foreach (var c in requiredClaims)
                {
                    var found = claimEntities.FindClaim(c.ClaimName);
                    if (found != null)
                    {
                        c.SyncTo(found);
                    }
                    else
                    {
                        var newClaim = new ClaimRequirementEntity();
                        newClaim.ClaimName = c.ClaimName;
                        newClaim.Policy = entity;
                        c.SyncTo(newClaim);
                        claimEntities.Add(newClaim);
                    }
                }


            }

        }

        public static void SyncTo(this ClaimRequirement claim, ClaimRequirementEntity entity)
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
                    }
                }


            }

        }

        public static bool HasRequiredValue(this List<AllowedClaimValueEntity> requiredClaims, string value)
        {
            foreach (var r in requiredClaims)
            {
                if (r.AllowedValue == value) { return true; }
            }

            return false;
        }


        public static bool HasClaim(this List<ClaimRequirement> requiredClaims, string claimName)
        {
            foreach (var r in requiredClaims)
            {
                if (r.ClaimName == claimName) { return true; }
            }

            return false;
        }

        public static ClaimRequirementEntity FindClaim(this List<ClaimRequirementEntity> requiredClaims, string claimName)
        {
            foreach (var r in requiredClaims)
            {
                if (r.ClaimName == claimName) { return r; }
            }

            return null;
        }


        public static void SyncTo(this List<string> allowedRoleStrings, List<AllowedRoleEntity> allowedRoles, AuthorizationPolicyEntity entity)
        {
            if (allowedRoleStrings.Count == 0)
            {
                allowedRoles.Clear();
            }
            else
            {
                if (allowedRoles.Count > 0)
                {
                    for (int i = 0; i < allowedRoles.Count; i++)
                    {
                        if (!allowedRoleStrings.Contains(allowedRoles[i].AllowedRole))
                        {
                            allowedRoles.RemoveAt(i);
                        }
                    }
                }

                foreach (var s in allowedRoleStrings)
                {
                    if (!allowedRoles.HasRole(s))
                    {
                        var r = new AllowedRoleEntity();
                        r.AllowedRole = s;
                        r.Policy = entity;
                        allowedRoles.Add(r);
                    }
                }

            }

        }

        public static bool HasRole(this List<AllowedRoleEntity> allowedRoles, string roleName)
        {
            foreach (var r in allowedRoles)
            {
                if (r.AllowedRole == roleName) { return true; }
            }

            return false;
        }

        public static void SyncTo(this List<string> authSchemeStrings, List<AuthenticationSchemeEntity> allowedSchemes, AuthorizationPolicyEntity entity)
        {
            if (authSchemeStrings.Count == 0)
            {
                allowedSchemes.Clear();
            }
            else
            {
                if (allowedSchemes.Count > 0)
                {
                    for (int i = 0; i < allowedSchemes.Count; i++)
                    {
                        if (!authSchemeStrings.Contains(allowedSchemes[i].AuthenticationScheme))
                        {
                            allowedSchemes.RemoveAt(i);
                        }
                    }
                }

                foreach (var s in authSchemeStrings)
                {
                    if (!allowedSchemes.HasScheme(s))
                    {
                        var r = new AuthenticationSchemeEntity();
                        r.AuthenticationScheme = s;
                        r.Policy = entity;
                        allowedSchemes.Add(r);
                    }
                }

            }

        }

        public static bool HasScheme(this List<AuthenticationSchemeEntity> authSchemes, string schemeName)
        {
            foreach (var r in authSchemes)
            {
                if (r.AuthenticationScheme == schemeName) { return true; }
            }

            return false;
        }

    }
}
