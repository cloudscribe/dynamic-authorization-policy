using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.PostgreSql.Migrations
{
    public partial class csdynamicpolicyinitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "csp_auth_policy",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    tenant_id = table.Column<string>(maxLength: 36, nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    require_authenticated_user = table.Column<bool>(nullable: false),
                    required_user_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csp_auth_policy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "csp_auth_policy_claim",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    claim_name = table.Column<string>(maxLength: 255, nullable: false),
                    policy_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csp_auth_policy_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_csp_auth_policy_claim_csp_auth_policy_policy_id",
                        column: x => x.policy_id,
                        principalTable: "csp_auth_policy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csp_auth_policy_role",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    allowed_role = table.Column<string>(maxLength: 200, nullable: false),
                    policy_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csp_auth_policy_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_csp_auth_policy_role_csp_auth_policy_policy_id",
                        column: x => x.policy_id,
                        principalTable: "csp_auth_policy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csp_auth_policy_scheme",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    authentication_scheme = table.Column<string>(maxLength: 255, nullable: false),
                    policy_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csp_auth_policy_scheme", x => x.id);
                    table.ForeignKey(
                        name: "fk_csp_auth_policy_scheme_csp_auth_policy_policy_id",
                        column: x => x.policy_id,
                        principalTable: "csp_auth_policy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csp_auth_policy_claim_value",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    allowed_value = table.Column<string>(maxLength: 255, nullable: false),
                    claim_requirement_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_csp_auth_policy_claim_value", x => x.id);
                    table.ForeignKey(
                        name: "fk_csp_auth_policy_claim_value_csp_auth_policy_claim_claim_requi~",
                        column: x => x.claim_requirement_id,
                        principalTable: "csp_auth_policy_claim",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_name",
                table: "csp_auth_policy",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_tenant_id",
                table: "csp_auth_policy",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_claim_policy_id",
                table: "csp_auth_policy_claim",
                column: "policy_id");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_claim_value_claim_requirement_id",
                table: "csp_auth_policy_claim_value",
                column: "claim_requirement_id");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_role_policy_id",
                table: "csp_auth_policy_role",
                column: "policy_id");

            migrationBuilder.CreateIndex(
                name: "ix_csp_auth_policy_scheme_policy_id",
                table: "csp_auth_policy_scheme",
                column: "policy_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csp_auth_policy_claim_value");

            migrationBuilder.DropTable(
                name: "csp_auth_policy_role");

            migrationBuilder.DropTable(
                name: "csp_auth_policy_scheme");

            migrationBuilder.DropTable(
                name: "csp_auth_policy_claim");

            migrationBuilder.DropTable(
                name: "csp_auth_policy");
        }
    }
}
