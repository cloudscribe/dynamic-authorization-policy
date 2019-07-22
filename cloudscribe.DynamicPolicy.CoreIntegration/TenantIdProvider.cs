using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using cloudscribe.DynamicPolicy.Models;

namespace cloudscribe.DynamicPolicy.CoreIntegration
{
    public class TenantIdProvider : ITenantIdProvider
    {
        public TenantIdProvider(
            IHttpContextAccessor httpContextAccessor
            )
        {
            _contextAccessor = httpContextAccessor;

        }
        
        private IHttpContextAccessor _contextAccessor;


        public string GetTenantId()
        {
            var tenant = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (tenant != null)
            {
                return tenant.Id.ToString();
            }

            var logger = _contextAccessor.HttpContext.RequestServices.GetService<ILogger<TenantIdProvider>>();
            logger.LogError($"failed to resolve tenant, returning literal word default as tenantid");

            return "default";
        }
    }
}
