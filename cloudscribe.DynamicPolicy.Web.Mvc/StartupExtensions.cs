using cloudscribe.DynamicPolicy.Models;
using cloudscribe.DynamicPolicy.Services;
using cloudscribe.DynamicPolicy.Web.Mvc;
using cloudscribe.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDynamicAuthorizationMvc(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            
            services.AddScoped<PolicyManagementService>();
            services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
            services.TryAddSingleton<ITenantIdProvider, DefaultTenantIdProvider>();
            services.Configure<PolicyManagementOptions>(configuration.GetSection("PolicyManagementOptions"));
            services.Configure<PolicyCacheOptions>(configuration.GetSection("PolicyCacheOptions"));
            services.AddScoped<PolicyCache>();
            
            services.TryAddScoped<IRoleSelectorProperties, NotImplementedRoleSelectorProperties>();
            
            services.AddScoped<IVersionProvider, VersionProvider>();

            return services;
        }

    }
}

