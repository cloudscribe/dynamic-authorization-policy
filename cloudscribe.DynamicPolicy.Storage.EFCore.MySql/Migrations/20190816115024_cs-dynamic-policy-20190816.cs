using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.MySql.Migrations
{
    public partial class csdynamicpolicy20190816 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_csp_AuthPolicy_Name",
                table: "csp_AuthPolicy");

            migrationBuilder.CreateIndex(
                name: "IX_csp_AuthPolicy_Name_TenantId",
                table: "csp_AuthPolicy",
                columns: new[] { "Name", "TenantId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_csp_AuthPolicy_Name_TenantId",
                table: "csp_AuthPolicy");

            migrationBuilder.CreateIndex(
                name: "IX_csp_AuthPolicy_Name",
                table: "csp_AuthPolicy",
                column: "Name");
        }
    }
}
