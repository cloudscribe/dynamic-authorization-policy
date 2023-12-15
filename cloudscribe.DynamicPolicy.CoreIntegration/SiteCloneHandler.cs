using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.DynamicPolicy.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.CoreIntegration
{
    public class SiteCloneHandler : IHandleSiteCloned
    {
        public SiteCloneHandler(IAuthorizationPolicyCommands policyCommands,
                                IAuthorizationPolicyQueries policyQueries)
        {
            _policyCommands = policyCommands;
            _policyQueries  = policyQueries;
        }

        private readonly IAuthorizationPolicyCommands _policyCommands;
        private readonly IAuthorizationPolicyQueries  _policyQueries;

        public async Task HandleSiteCloned(
            ISiteSettings newSite,
            ISiteSettings sourceSite,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var policies = await _policyQueries.GetAll(sourceSite.Id.ToString());
            var policiesnew = await _policyQueries.GetAll(newSite.Id.ToString());

            foreach (var policy in policies)
            {
                var exists = await _policyQueries.Fetch(newSite.Id.ToString(), policy.Name);
                if(exists == null)
                {
                    policy.Id = Guid.NewGuid();
                    policy.TenantId = newSite.Id.ToString();
                    await _policyCommands.Create(policy);
                }
            }
        }
    }
}
