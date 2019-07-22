using cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common
{
    public class DynamicPolicyDbContextBase : DbContext
    {
        public DynamicPolicyDbContextBase(
            DbContextOptions options) : base(options)
        {

        }

        public DbSet<AuthorizationPolicyEntity> Policies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
