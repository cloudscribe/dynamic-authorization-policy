using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.MySql
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<DynamicPolicyDbContext>
    {
        public DynamicPolicyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DynamicPolicyDbContext>();
            builder.UseMySql("Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;");
            return new DynamicPolicyDbContext(builder.Options);
        }

    }
}
