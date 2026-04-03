using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.DynamicPolicy.Storage.EFCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class csdynamicpolicynet10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_csp_auth_policy_claim_value_csp_auth_policy_claim_claim_requi~",
                table: "csp_auth_policy_claim_value");

            migrationBuilder.AddForeignKey(
                name: "fk_csp_auth_policy_claim_value_csp_auth_policy_claim_claim_requi",
                table: "csp_auth_policy_claim_value",
                column: "claim_requirement_id",
                principalTable: "csp_auth_policy_claim",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_csp_auth_policy_claim_value_csp_auth_policy_claim_claim_requi",
                table: "csp_auth_policy_claim_value");

            migrationBuilder.AddForeignKey(
                name: "fk_csp_auth_policy_claim_value_csp_auth_policy_claim_claim_requi~",
                table: "csp_auth_policy_claim_value",
                column: "claim_requirement_id",
                principalTable: "csp_auth_policy_claim",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
