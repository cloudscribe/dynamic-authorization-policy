using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.PostgreSql
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<DynamicPolicyDbContext>
    {
        public DynamicPolicyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DynamicPolicyDbContext>();
            builder.UseNpgsql("server=yourservername;UID=yourdatabaseusername;PWD=yourdatabaseuserpassword;database=yourdatabasename");
            return new DynamicPolicyDbContext(builder.Options);
        }

    }
}
