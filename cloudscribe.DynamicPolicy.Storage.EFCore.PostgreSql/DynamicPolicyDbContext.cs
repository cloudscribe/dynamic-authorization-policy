using cloudscribe.DynamicPolicy.Storage.EFCore.Common;
using cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities;
using cloudscribe.EFCore.PostgreSql.Conventions;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.PostgreSql
{
    public class DynamicPolicyDbContext : DynamicPolicyDbContextBase, IDynamicPolicyDbContext
    {
        public DynamicPolicyDbContext(
            DbContextOptions<DynamicPolicyDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorizationPolicyEntity>(entity =>
            {
                entity.ToTable("csp_AuthPolicy").HasKey(x => x.Id);

                entity.Property(x => x.TenantId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.TenantId);

                entity.Property(x => x.Name).HasMaxLength(200);
                entity.HasIndex(x => new { x.Name, x.TenantId }).IsUnique();

                entity.HasMany(x => x.AllowedRoles).WithOne(x => x.Policy).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.AuthenticationSchemes).WithOne(x => x.Policy).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.RequiredClaims).WithOne(x => x.Policy).IsRequired().OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<AllowedRoleEntity>(entity =>
            {
                entity.ToTable("csp_AuthPolicyRole").HasKey(x => x.Id);
                entity.Property(x => x.AllowedRole).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<AuthenticationSchemeEntity>(entity =>
            {
                entity.ToTable("csp_AuthPolicyScheme").HasKey(x => x.Id);
                entity.Property(x => x.AuthenticationScheme).HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<ClaimRequirementEntity>(entity =>
            {
                entity.ToTable("csp_AuthPolicyClaim").HasKey(x => x.Id);
                entity.Property(x => x.ClaimName).HasMaxLength(255).IsRequired();
                entity.HasMany(x => x.AllowedValues).WithOne(x => x.ClaimRequirement).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AllowedClaimValueEntity>(entity =>
            {
                entity.ToTable("csp_AuthPolicyClaimValue").HasKey(x => x.Id);
                entity.Property(x => x.AllowedValue).HasMaxLength(255).IsRequired();

            });

            modelBuilder.ApplySnakeCaseConventions();

        }

    }
}
