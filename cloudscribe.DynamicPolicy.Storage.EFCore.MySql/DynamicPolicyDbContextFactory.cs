using cloudscribe.DynamicPolicy.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.MySql
{
    public class DynamicPolicyDbContextFactory : IDynamicPolicyDbContextFactory
    {
        public DynamicPolicyDbContextFactory(DbContextOptions<DynamicPolicyDbContext> options)
        {
            _options = options;
        }

        private readonly DbContextOptions<DynamicPolicyDbContext> _options;

        public IDynamicPolicyDbContext CreateContext()
        {
            return new DynamicPolicyDbContext(_options);
        }

    }
}
