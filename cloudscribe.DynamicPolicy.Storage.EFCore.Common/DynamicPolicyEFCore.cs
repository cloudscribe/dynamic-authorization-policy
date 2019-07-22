using cloudscribe.DynamicPolicy.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Hosting
{
    public static class DynamicPolicyEFCore
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider scopedServices)
        {
            var db = scopedServices.GetService<IDynamicPolicyDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}
