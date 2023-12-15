using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.DynamicPolicy.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.CoreIntegration
{
    public class SiteDeleteHandler : IHandleSitePreDelete
    {
        public SiteDeleteHandler(IAuthorizationPolicyCommands policyCommands)
        {
            _policyCommands = policyCommands;
        }

        private IAuthorizationPolicyCommands _policyCommands;

        public async Task HandleSitePreDelete(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await _policyCommands.DeleteByTenant(siteId.ToString());
        }
    }
}
