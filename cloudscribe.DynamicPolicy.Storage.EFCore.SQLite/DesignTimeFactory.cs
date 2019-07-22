using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.SQLite
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<DynamicPolicyDbContext>
    {
        public DynamicPolicyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DynamicPolicyDbContext>();
            builder.UseSqlite("Data Source=database.db");
            return new DynamicPolicyDbContext(builder.Options);
        }

    }
}
