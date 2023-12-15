using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Models
{
    public interface IAuthorizationPolicyQueries
    {
        Task<AuthorizationPolicyInfo> Fetch(
            string tenantId,
            Guid policyId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<AuthorizationPolicyInfo> Fetch(
            string tenantId,
            string policyName,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<AuthorizationPolicyInfo>> GetPage(
            string tenantId,
            string searchQuery,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<AuthorizationPolicyInfo>> GetAll(
            string tenantId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
