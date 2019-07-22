using cloudscribe.DynamicPolicy.Storage.EFCore.Common;
using cloudscribe.DynamicPolicy.Storage.EFCore.SQLite;
using cloudscribe.Versioning;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDynamicPolicyEFStorageSQLite(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddDbContext<DynamicPolicyDbContext>(options =>
                    options.UseSqlite(connectionString),
                    optionsLifetime: ServiceLifetime.Singleton
                    );

            services.AddScoped<IDynamicPolicyDbContext, DynamicPolicyDbContext>();
            services.AddSingleton<IDynamicPolicyDbContextFactory, DynamicPolicyDbContextFactory>();

            services.AddDynamicPolicyEFStorageCommon();
            services.AddScoped<IVersionProvider, VersionProvider>();

            return services;
        }
    }
}
