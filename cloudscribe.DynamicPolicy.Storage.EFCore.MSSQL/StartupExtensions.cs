using cloudscribe.DynamicPolicy.Storage.EFCore.Common;
using cloudscribe.DynamicPolicy.Storage.EFCore.MSSQL;
using cloudscribe.Versioning;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDynamicPolicyEFStorageMSSQL(
            this IServiceCollection services,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<int> transientSqlErrorNumbersToAdd = null
            // bool useSql2008Compatibility = false
            )
        {
            
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<DynamicPolicyDbContext>(options =>
                    options.UseSqlServer(connectionString,
                   sqlServerOptionsAction: sqlOptions =>
                   {
                       if (maxConnectionRetryCount > 0)
                       {
                           //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                           sqlOptions.EnableRetryOnFailure(
                               maxRetryCount: maxConnectionRetryCount,
                               maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                               errorNumbersToAdd: transientSqlErrorNumbersToAdd);
                       }
                   }),
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
