using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.PostgreSql.Migrations
{
    public partial class csdynamicpolicy20190816 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_csp_auth_policy_name",
                table: "csp_auth_policy");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_name_tenant_id",
                table: "csp_auth_policy",
                columns: new[] { "name", "tenant_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_csp_auth_policy_name_tenant_id",
                table: "csp_auth_policy");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_name",
                table: "csp_auth_policy",
                column: "name");
        }
    }
}
