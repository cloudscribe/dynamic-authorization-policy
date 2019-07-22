using cloudscribe.DynamicPolicy.Models;
using Microsoft.Extensions.DependencyInjection;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDynamicPolicyEFStorageCommon(this IServiceCollection services)
        {

            services.AddScoped<IAuthorizationPolicyCommands, AuthorizationPolicyCommands>();
            services.AddScoped<IAuthorizationPolicyQueries, AuthorizationPolicyQueries>();

            return services;
        }

    }
}
