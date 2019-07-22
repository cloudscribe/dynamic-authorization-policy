using cloudscribe.DynamicPolicy.Models;
using cloudscribe.DynamicPolicy.Storage.NoDb;
using cloudscribe.Versioning;
using Microsoft.Extensions.Configuration;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddNoDbStorageForDynamicPolicies(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddNoDb<AuthorizationPolicyInfo>();
            services.AddScoped<IAuthorizationPolicyCommands, AuthorizationPolicyCommands>();
            services.AddScoped<IAuthorizationPolicyQueries, AuthorizationPolicyQueries>();
            services.AddScoped<IVersionProvider, VersionProvider>();


            return services;
        }

    }
}
