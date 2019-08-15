using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.PostgreSql.Migrations
{
    public partial class csdynamicpolicy20190815 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_csp_auth_policy_name",
                table: "csp_auth_policy");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_name",
                table: "csp_auth_policy",
                column: "name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_csp_auth_policy_name",
                table: "csp_auth_policy");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_name",
                table: "csp_auth_policy",
                column: "name");
        }
    }
}
