// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:					2018-01-02
// Last Modified:			2019-04-17
// 

using Microsoft.AspNetCore.Authorization;
using cloudscribe.DynamicPolicy.Models;
using System;

namespace cloudscribe.DynamicPolicy.Services
{
    public static class ModelExtensions
    {
        public static AuthorizationPolicy ToAuthPolicy(this AuthorizationPolicyInfo info)
        {
            var policy = new AuthorizationPolicyBuilder();
            var hasAnyRequirements = false;

            if (info.AllowedRoles.Count > 0)
            {
                policy.RequireRole(info.AllowedRoles);
                hasAnyRequirements = true;
            }

            if (info.AuthenticationSchemes.Count > 0)
            {
                policy.AuthenticationSchemes = info.AuthenticationSchemes;
            }
            if (info.RequireAuthenticatedUser)
            {
                policy.RequireAuthenticatedUser();
                hasAnyRequirements = true;
            }
            if (info.RequiredClaims.Count > 0)
            {
                foreach (var c in info.RequiredClaims)
                {
                    if (c.AllowedValues.Count > 0)
                    {
                        policy.RequireClaim(c.ClaimName, c.AllowedValues);
                    }
                    else
                    {
                        policy.RequireClaim(c.ClaimName);
                    }
                    hasAnyRequirements = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(info.RequiredUserName))
            {
                policy.RequireUserName(info.RequiredUserName);
                hasAnyRequirements = true;
            }

            if(!hasAnyRequirements)
            {
                // allow anonymous
                Func<AuthorizationHandlerContext, bool> allowAny = (AuthorizationHandlerContext authContext) => true;
                policy.RequireAssertion(allowAny);
            }


            return policy.Build();
        }

        public static bool HasClaimRequirement(this AuthorizationPolicyInfo info, string claimName)
        {
            foreach (var req in info.RequiredClaims)
            {
                if (req.ClaimName == claimName) { return true; }
            }

            return false;
        }

        public static ClaimRequirement GetClaimRequirement(this AuthorizationPolicyInfo info, string claimName)
        {
            foreach (var req in info.RequiredClaims)
            {
                if (req.ClaimName == claimName) { return req; }
            }

            return null;
        }

        public static bool RemoveClaimRequirement(this AuthorizationPolicyInfo info, string claimName)
        {
            for (int i = 0; i < info.RequiredClaims.Count; i++)
            {
                if (info.RequiredClaims[i].ClaimName == claimName)
                {
                    info.RequiredClaims.RemoveAt(i);
                    return true;

                }
            }

            return false;
        }


    }
}
