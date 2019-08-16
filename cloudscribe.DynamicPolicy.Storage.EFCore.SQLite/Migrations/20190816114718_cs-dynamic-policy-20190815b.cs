using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.SQLite.Migrations
{
    public partial class csdynamicpolicy20190815b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_csp_AuthPolicy_Name",
            //    table: "csp_AuthPolicy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_csp_AuthPolicy_Name",
            //    table: "csp_AuthPolicy",
            //    column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_csp_AuthPolicy_Name",
            //    table: "csp_AuthPolicy");

            //migrationBuilder.CreateIndex(
            //    name: "IX_csp_AuthPolicy_Name",
            //    table: "csp_AuthPolicy",
            //    column: "Name",
            //    unique: true);
        }
    }
}
