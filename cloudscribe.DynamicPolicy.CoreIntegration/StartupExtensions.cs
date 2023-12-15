using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.DynamicPolicy.CoreIntegration;
using cloudscribe.DynamicPolicy.Models;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeDynamicPolicyIntegration(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddSingleton<ITenantIdProvider, TenantIdProvider>();

            services.AddScoped<IRoleSelectorProperties, SiteRoleSelectorProperties>();

            services.AddScoped<IHandleSitePreDelete, SiteDeleteHandler>();
            services.AddScoped<IHandleSiteCloned, SiteCloneHandler>();
            services.AddScoped<IGuardNeededRoles, AuthPolicyRoleGuard>();
            

            return services;
        }
    }
}
