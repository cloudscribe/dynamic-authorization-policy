using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.DynamicPolicy.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.CoreIntegration
{
    public class RoleCopiedHandler : IHandleRoleCopied
    {
        public RoleCopiedHandler(
            IAuthorizationPolicyCommands policyCommands,
            IAuthorizationPolicyQueries policyQueries,
            ILogger<RoleCopiedHandler> logger)
        {
            _policyCommands = policyCommands;
            _policyQueries = policyQueries;
            _log = logger;
        }

        private readonly IAuthorizationPolicyCommands _policyCommands;
        private readonly IAuthorizationPolicyQueries _policyQueries;
        private readonly ILogger _log;

        public async Task Handle(ISiteRole sourceRole, ISiteRole newRole)
        {
            try
            {
                var tenantId = sourceRole.SiteId.ToString();
                var policies = await _policyQueries.GetAll(tenantId);
                
                int updatedCount = 0;
                foreach (var policy in policies)
                {
                    if (policy.AllowedRoles.Contains(sourceRole.RoleName))
                    {
                        if (!policy.AllowedRoles.Contains(newRole.RoleName))
                        {
                            policy.AllowedRoles.Add(newRole.RoleName);
                            await _policyCommands.Update(policy);
                            updatedCount++;
                        }
                    }
                }
                
                _log.LogInformation($"Role '{sourceRole.RoleName}' copied to '{newRole.RoleName}'. Updated {updatedCount} authorization policies.");
            }
            catch (Exception ex)
            {
                _log.LogError($"Error handling role copied event: {ex.Message}-{ex.StackTrace}");
            }
        }
    }
}
