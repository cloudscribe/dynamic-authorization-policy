// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:					2018-01-01
// Last Modified:			2019-03-25
// 

using cloudscribe.Pagination.Models;
using Microsoft.Extensions.Logging;
using cloudscribe.DynamicPolicy.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Services
{
    public class PolicyManagementService
    {
        public PolicyManagementService(
            ITenantIdProvider tenantProvider,
            IAuthorizationPolicyCommands commands,
            IAuthorizationPolicyQueries queries,
            ILogger<PolicyManagementService> logger
            )
        {
            _tenantProvider = tenantProvider;
            _commands = commands;
            _queries = queries;
            _log = logger;
        }

        private ITenantIdProvider _tenantProvider;
        private IAuthorizationPolicyCommands _commands;
        private IAuthorizationPolicyQueries _queries;
        private ILogger _log;

        public async Task<AuthorizationPolicyInfo> FetchPolicy(
            Guid policyId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _queries.Fetch(_tenantProvider.GetTenantId(), policyId).ConfigureAwait(false);
        }

        public async Task<AuthorizationPolicyInfo> FetchPolicy(
            string policyName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _queries.Fetch(_tenantProvider.GetTenantId(), policyName).ConfigureAwait(false);
        }

        public async Task<PagedResult<AuthorizationPolicyInfo>> GetPageOfPolicies(
            string query = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _queries.GetPage(_tenantProvider.GetTenantId(), query, pageNumber, pageSize, cancellationToken).ConfigureAwait(false);

            //if(!_lm.IA())
            //{
            //    var a = new AuthorizationPolicyInfo();
            //    a.Name = $"This site is using a trial version of Dynamic Authorization Policies. {_lm.GetAu()}";


            //    result.Data.Add(a);
            //}

            return result;

        }

        public async Task<PolicyOperationResult> CreatePolicy(
            AuthorizationPolicyInfo policy,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (policy == null) { throw new ArgumentException("policy cannot be null"); }
            policy.TenantId = _tenantProvider.GetTenantId();
            await _commands.Create(policy, cancellationToken).ConfigureAwait(false);

            return new PolicyOperationResult(true);
        }

        public async Task<PolicyOperationResult> UpdatePolicy(
            Guid policyId,
            bool requireAuthenticatedUser,
            string allowedRolesCsv,
            string authenticationSchemesCsv,
            string requiredUserName,
            string notes
            )
        {
            var policy = await FetchPolicy(policyId);
            if (policy == null)
            {
                var message = $"failed to find policy with id {policyId} so could not update";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }

            policy.RequireAuthenticatedUser = requireAuthenticatedUser;
            policy.RequiredUserName = requiredUserName;
            policy.Notes = notes;
            policy.AllowedRoles.Clear();
            policy.AuthenticationSchemes.Clear();

            if (!string.IsNullOrWhiteSpace(allowedRolesCsv))
            {
                var newRoles = allowedRolesCsv.Split(',');
                if (newRoles.Length > 0)
                {
                    foreach (var role in newRoles)
                    {
                        policy.AllowedRoles.Add(role.Trim());
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(authenticationSchemesCsv))
            {
                var newSchemes = authenticationSchemesCsv.Split(',');
                if (newSchemes.Length > 0)
                {
                    foreach (var scheme in newSchemes)
                    {
                        policy.AuthenticationSchemes.Add(scheme.Trim());
                    }
                }
            }

            await _commands.Update(policy).ConfigureAwait(false);

            return new PolicyOperationResult(true);
        }

        public async Task<PolicyOperationResult> DeletePolicy(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _commands.Delete(_tenantProvider.GetTenantId(), id, cancellationToken).ConfigureAwait(false);

            return new PolicyOperationResult(true);
        }

        public async Task<PolicyOperationResult> AddClaimRequirement(Guid policyId, string claimName, string allowedValuesCsv)
        {
            string message;
            if (string.IsNullOrWhiteSpace(claimName))
            {
                message = $"claimName was empty, failed to add claim requirement";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }
            var policy = await FetchPolicy(policyId);
            if (policy == null)
            {
                message = $"failed to find policy with id {policyId} so could not add claim requirement {claimName}";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }

            var claimRequirement = new ClaimRequirement();
            claimRequirement.ClaimName = claimName;
            if (!string.IsNullOrEmpty(allowedValuesCsv))
            {
                var vals = allowedValuesCsv.Split(',');
                if (vals.Length > 0)
                {
                    foreach (var v in vals)
                    {
                        claimRequirement.AllowedValues.Add(v.Trim());
                    }
                }
            }
            if (!policy.HasClaimRequirement(claimRequirement.ClaimName))
            {
                policy.RequiredClaims.Add(claimRequirement);
                await _commands.Update(policy);
            }
            else
            {
                message = $"tried to add claim {claimName} to policy {policy.Name}, but it already has a claim requirement for that claim name";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }

            return new PolicyOperationResult(true);
        }

        public async Task<PolicyOperationResult> UpdateClaimRequirement(Guid policyId, string claimName, string allowedValuesCsv)
        {
            string message;
            if (string.IsNullOrWhiteSpace(claimName))
            {
                message = $"claimName was empty, failed to update claim requirement";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }
            var policy = await FetchPolicy(policyId);
            if (policy == null)
            {
                message = $"failed to find policy with id {policyId} so could not update claim requirement {claimName}";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }

            var claimRequirement = policy.GetClaimRequirement(claimName);
            if (claimRequirement != null)
            {
                claimRequirement.AllowedValues.Clear();

                if (!string.IsNullOrEmpty(allowedValuesCsv))
                {
                    var vals = allowedValuesCsv.Split(',');
                    if (vals.Length > 0)
                    {
                        foreach (var v in vals)
                        {
                            claimRequirement.AllowedValues.Add(v.Trim());
                        }
                    }
                }

                await _commands.Update(policy);
            }
            else
            {
                message = $"tried to update claim {claimName} on policy {policy.Name}, but no claim requirement with that name was found on the policy";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }

            return new PolicyOperationResult(true);
        }

        public async Task<PolicyOperationResult> DeleteClaimRequirement(Guid policyId, string claimName)
        {
            string message;
            if (string.IsNullOrWhiteSpace(claimName))
            {
                message = $"claimName was empty, failed to delete claim requirement";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }

            var policy = await FetchPolicy(policyId);
            if (policy == null)
            {
                message = $"failed to find policy with id {policyId} so could not delete claim requirement {claimName}";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }

            if (policy.RemoveClaimRequirement(claimName))
            {
                await _commands.Update(policy);
            }
            else
            {
                message = $"tried to delete claim {claimName} on policy {policy.Name}, but no claim requirement with that name was found on the policy";
                _log.LogError(message);
                return new PolicyOperationResult(false, message);
            }

            return new PolicyOperationResult(true);
        }

    }
}
