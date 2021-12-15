using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.MySql
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<DynamicPolicyDbContext>
    {
        public DynamicPolicyDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DynamicPolicyDbContext>();
            var conn = "Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;";
            builder.UseMySql(conn, ServerVersion.AutoDetect(conn)); // breaking change in Net5.0
            return new DynamicPolicyDbContext(builder.Options);
        }
    }
}
