// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:					2018-06-16
// Last Modified:			2019-03-25
// 

using cloudscribe.Core.Models;
using cloudscribe.DynamicPolicy.Models;
using cloudscribe.DynamicPolicy.Web.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.CoreIntegration
{
    public class AuthPolicyRoleGuard : IGuardNeededRoles
    {
        public AuthPolicyRoleGuard(
            IAuthorizationPolicyQueries queries,
            IStringLocalizer<PolicyResources> localizer,
            ILogger<AuthPolicyRoleGuard> logger
            )
        {
            _queries = queries;
            _localizer = localizer;
            _log = logger;
        }

        private readonly IAuthorizationPolicyQueries _queries;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger _log;
        private const string reasonFormat = "The role {0} cannot be edited or removed because it is in use on one or more authorization policies.";

        private async Task<bool> PolicyExistWithRole(Guid siteId, string role)
        {
            var all = await _queries.GetPage(siteId.ToString(), null, 1, 900000).ConfigureAwait(false);
            for (var i = 0; i < all.Data.Count; i++)
            {
                var policy = all.Data[i];
                if (policy.AllowedRoles.Contains(role))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<string> GetEditRejectReason(Guid siteId, string role)
        {
            var isUsed = await PolicyExistWithRole(siteId, role).ConfigureAwait(false);
            if (isUsed)
            {
                return string.Format(_localizer[reasonFormat], role);
            }
            
            return null;
        }

    }
}
