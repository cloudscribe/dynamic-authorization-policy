using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Models
{
    public interface IAuthorizationPolicyCommands
    {
        Task Create(
            AuthorizationPolicyInfo policy,
            CancellationToken cancellationToken = default(CancellationToken));

        Task Update(
            AuthorizationPolicyInfo policy,
            CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(
            string tenantId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteByTenant(
            string tenantId,
            CancellationToken cancellationToken = default(CancellationToken));



    }

}
