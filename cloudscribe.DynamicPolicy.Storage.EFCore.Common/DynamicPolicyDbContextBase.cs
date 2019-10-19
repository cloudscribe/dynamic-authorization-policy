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

        public DbSet<AllowedRoleEntity> AllowedRoles { get; set; }

        public DbSet<AuthenticationSchemeEntity> AuthenticationSchemes { get; set; }

        public DbSet<ClaimRequirementEntity> ClaimRequirements { get; set; }

        public DbSet<AllowedClaimValueEntity> AllowedClaimValues { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
