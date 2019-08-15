// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:					2018-01-01
// Last Modified:			2019-07-31
// 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using cloudscribe.DynamicPolicy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

//https://www.jerriepelser.com/blog/creating-dynamic-authorization-policies-aspnet-core/
//https://docs.microsoft.com/en-us/aspnet/core/security/authorization/

//https://github.com/aspnet/home/releases
/*
NullReferenceException when using a custom IAuthorizationPolicyProvider and AllowCombiningAuthorizeFilters is set to true
Using a custom IAuthorizationPolicyProvider and setting AllowCombiningAuthorizeFilters to true causes a NullReferenceException.
Workaround: Disable AllowCombiningAuthorizeFilters or use a single AuthorizeFilter
Default value is false
https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.mvcoptions.allowcombiningauthorizefilters?view=aspnetcore-2.1
 */
//https://brockallen.com/2018/07/15/beware-the-combined-authorize-filter-mechanics-in-asp-net-core-2-1/

namespace cloudscribe.DynamicPolicy.Services
{
    // keep in mind this is injected as a singleton
    // any scoped dependencies need to be obtained as needed rather than injected
    public class DynamicAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {


        public DynamicAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            IOptions<PolicyManagementOptions> managementOptionsAccessor,
            ITenantIdProvider tenantIdResolver,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration
            ) : base(options)
        {
            _policyOptions = managementOptionsAccessor.Value;
            _tenantIdResolver = tenantIdResolver;
            _contextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private PolicyManagementOptions _policyOptions;
        private ITenantIdProvider _tenantIdResolver;
        private IHttpContextAccessor _contextAccessor;

        private readonly IConfiguration _configuration;

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // Check static policies first
            var policy = await base.GetPolicyAsync(policyName);

            if (policy == null)
            {
                var policyService = _contextAccessor.HttpContext.RequestServices.GetService<PolicyManagementService>();
                var policyInfo = await policyService.FetchPolicy(policyName);
                if (policyInfo != null)
                {
                    return policyInfo.ToAuthPolicy();
                }

                if (_policyOptions.AutoCreateMissingPolicies)
                {
                    //initialize policy in the data storage
                    var newPolicy = new AuthorizationPolicyInfo();
                    newPolicy.Name = policyName;

                    if (_policyOptions.PolicyNamesToConfigureAsAllowAnonymous.Contains(policyName))
                    {
                        await policyService.CreatePolicy(newPolicy);
                        return newPolicy.ToAuthPolicy();
                    }
                    else if(_policyOptions.PolicyNamesToConfigureAsAnyAuthenticatedUser.Contains(policyName))
                    {
                        newPolicy.RequireAuthenticatedUser = true;
                        await policyService.CreatePolicy(newPolicy);
                        return newPolicy.ToAuthPolicy();
                    }
                    else
                    {
                        var allowedRoles = _policyOptions.AutoPolicyAllowedRoleNamesCsv.Split(',');
                        
                        var roleList = new List<string>(allowedRoles);
                        newPolicy.AllowedRoles = roleList;
                        await policyService.CreatePolicy(newPolicy);

                        policy = new AuthorizationPolicyBuilder()
                            .RequireRole(allowedRoles)
                            .Build();
                    }
                    
                    var logger = _contextAccessor.HttpContext.RequestServices.GetService<ILogger<DynamicAuthorizationPolicyProvider>>();
                    logger.LogWarning($"policy named {policyName} was missing so auto creating it with default allowed roles");

                }

            }

            return policy;
        }
    }
}
