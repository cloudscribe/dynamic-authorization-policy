using cloudscribe.DynamicPolicy.Storage.EFCore.Common;
using cloudscribe.DynamicPolicy.Storage.EFCore.MySql;
using cloudscribe.Versioning;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDynamicPolicyEFStorageMySql(
            this IServiceCollection services,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<int> transientSqlErrorNumbersToAdd = null
            )
        {
            
            services // .AddEntityFrameworkMySql()
                .AddDbContext<DynamicPolicyDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), // breaking change in Net5.0
                    mySqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

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
