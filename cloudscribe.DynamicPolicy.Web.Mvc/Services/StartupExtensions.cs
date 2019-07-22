// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:					2018-01-01
// Last Modified:			2018-01-04
// 

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using cloudscribe.DynamicPolicy.Models;

namespace cloudscribe.DynamicPolicy.Services
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDynamicAuthorizationServices(
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


            return services;
        }

    }
}
